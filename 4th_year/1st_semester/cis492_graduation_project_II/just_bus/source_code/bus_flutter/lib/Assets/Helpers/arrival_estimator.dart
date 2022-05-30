import 'dart:collection';
import 'dart:math';

import 'package:bus/Assets/Enums/path_types.dart';
import 'package:bus/Assets/Enums/weather.dart';
import 'package:bus/Assets/Models/Providers/Home/drivers_locations.dart';
import 'package:bus/Assets/Models/Providers/Home/map_properties.dart';
import 'package:bus/Assets/Models/Providers/Home/user_location.dart';
import 'package:bus/Assets/Models/map_route.dart';
import 'package:bus/Assets/Models/server_status.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_translate/flutter_translate.dart';
import 'package:geolocator/geolocator.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:maps_toolkit/maps_toolkit.dart' as toolkit;
import 'package:provider/provider.dart';

class ArrivalEstimator {
  static late MapRoute _selectedRoute;
  static List<toolkit.LatLng>? _outgoing;
  static List<toolkit.LatLng>? _incoming;
  static double _totalDistance = 0;
  static toolkit.LatLng? _lastLatLng;
  static final List<toolkit.LatLng> _driversLocationsInnerRoute = [];
  static late BuildContext _context;
  static int _markerType = -1;
  static double _outgoingDistancePercentageCovered = 0,
      _incomingDistancePercentageCovered = 0;
  static bool isUserADriver = false;

  static Future<EstimationData> getEstimation(
      MapRoute selectedRoute,
      BuildContext context,
      final MarkerId markerId,
      GoogleMapController mapController,
      [bool isUserADriver = false]) async {
    ArrivalEstimator.isUserADriver = isUserADriver;
    EstimationData estimationData = EstimationData('', '', '');

    _selectedRoute = selectedRoute;
    _context = context;

    final Marker marker = _context
        .read<MapProperties>()
        .markers
        .where((marker) => marker.markerId == markerId)
        .first;
    _markerType = _isSelectedMarkerAStop(marker);

    if (_markerType != -1) {
      List<toolkit.LatLng> poly = [];

      _outgoing = _context
          .read<MapProperties>()
          .polylines
          .first
          .points
          .map((point) => toolkit.LatLng(point.latitude, point.longitude))
          .toList();
      _incoming = _context
          .read<MapProperties>()
          .polylines
          .last
          .points
          .map((point) => toolkit.LatLng(point.latitude, point.longitude))
          .toList();

      _totalDistance = 0;
      _driversLocationsInnerRoute.clear();
      for (toolkit.LatLng latLng in getActiveDrivers()) {
        if (toolkit.PolygonUtil.isLocationOnPath(latLng, _outgoing!, false,
                tolerance: 5.0) ||
            toolkit.PolygonUtil.isLocationOnPath(latLng, _incoming!, false,
                tolerance: 5.0)) _driversLocationsInnerRoute.add(latLng);
      }

      if (_driversLocationsInnerRoute.isEmpty) {
        return estimationData;
      }

      _removeDuplicates(_outgoing!);
      _removeDuplicates(_incoming!);

      poly.addAll(_outgoing!);
      poly.addAll(_incoming!);

      List<int> nearestBusPoint = _getNearestBusPoint(poly, marker, _incoming!);

      if (nearestBusPoint[0] > nearestBusPoint[1]) {
        poly = poly.sublist(nearestBusPoint[1], nearestBusPoint[0] + 1);
      } else {
        var temp = poly.sublist(nearestBusPoint[1]);
        temp.addAll(poly.sublist(0, nearestBusPoint[0] + 1));
        poly = temp;
      }

      for (int i = 1; i < poly.length; i++) {
        _lastLatLng = poly[i - 1];
        _totalDistance += Geolocator.distanceBetween(poly[i].latitude,
            poly[i].longitude, _lastLatLng!.latitude, _lastLatLng!.longitude);
      }

      double _outgoingDistanceCovered = 0;
      double _incomingDistanceCovered = 0;
      for (int i = 1; i < poly.length; i++) {
        _lastLatLng = poly[i - 1];
        if (_outgoing!.contains(poly[i])) {
          _outgoingDistanceCovered += Geolocator.distanceBetween(
              poly[i].latitude,
              poly[i].longitude,
              _lastLatLng!.latitude,
              _lastLatLng!.longitude);
        } else {
          _incomingDistanceCovered += Geolocator.distanceBetween(
              poly[i].latitude,
              poly[i].longitude,
              _lastLatLng!.latitude,
              _lastLatLng!.longitude);
        }
      }

      _outgoingDistancePercentageCovered =
          _outgoingDistanceCovered / _totalDistance;
      _incomingDistancePercentageCovered =
          _incomingDistanceCovered / _totalDistance;
    }

    return EstimationData(
        _getFormattedDistance((_totalDistance).round()),
        _getEstimatedTime((_totalDistance).round()),
        _getFormattedSpeed().toString());
  }

  static void _removeDuplicates(List<toolkit.LatLng> linkedList) {
    LinkedHashSet<toolkit.LatLng> s = LinkedHashSet<toolkit.LatLng>();

    s.addAll(linkedList);
    linkedList.clear();
    linkedList.addAll(s);
  }

  static List<int> _getNearestBusPoint(List<toolkit.LatLng> poly,
      Marker stopMarker, List<toolkit.LatLng> returningSubstitute) {
    List<int> nearestStopAndBus = [];

    List<int> driverPoints = [];

    for (toolkit.LatLng latLng in _driversLocationsInnerRoute) {
      bool isDeparture;
      List<double> distance = [];

      if (isDriverOnDepartureSide(latLng, _context) == 1) {
        for (int i = 0; i < _outgoing!.length; i++) {
          distance.add(_getDistance(poly[i], latLng));
        }
        isDeparture = true;
      } else {
        for (int i = _outgoing!.length; i < poly.length; i++) {
          distance.add(_getDistance(poly[i], latLng));
        }
        isDeparture = false;
      }

      if (isDeparture) {
        driverPoints.add(distance.indexOf(distance.reduce(min)));
      } else {
        driverPoints
            .add(_outgoing!.length + distance.indexOf(distance.reduce(min)));
      }
    }

    driverPoints.sort();

    nearestStopAndBus.add(_getStopPoint(poly, stopMarker, returningSubstitute));

    nearestStopAndBus
        .add(_getNearestDriver(nearestStopAndBus[0], driverPoints));

    return nearestStopAndBus;
  }

  static int isDriverOnDepartureSide(
      toolkit.LatLng latLng, BuildContext context) {
    List<toolkit.LatLng> outgoing = context
        .read<MapProperties>()
        .polylines
        .first
        .points
        .map((point) => toolkit.LatLng(point.latitude, point.longitude))
        .toList();

    List<toolkit.LatLng> incoming = context
        .read<MapProperties>()
        .polylines
        .last
        .points
        .map((point) => toolkit.LatLng(point.latitude, point.longitude))
        .toList();

    for (int i = 0; i < 50; i++) {
      if (toolkit.PolygonUtil.isLocationOnPath(latLng, outgoing, false,
          tolerance: i / 10.0)) return 1;
      if (toolkit.PolygonUtil.isLocationOnPath(latLng, incoming, false,
          tolerance: i / 10.0)) return 0;
    }
    return -1;
  }

  static int _getStopPoint(List<toolkit.LatLng> poly, Marker marker,
      List<toolkit.LatLng> returningSubstitute) {
    int startingPoint = (_markerType == 0) ? 0 : _outgoing!.length;
    int endingPoint = (_markerType == 0)
        ? _outgoing!.length
        : _outgoing!.length + returningSubstitute.length;

    List<double> pointsWithDistance = [];
    for (int i = startingPoint; i < endingPoint; i++) {
      pointsWithDistance.add(_getDistance(poly[i],
          toolkit.LatLng(marker.position.latitude, marker.position.longitude)));
    }

    int stopPoint = pointsWithDistance.indexOf(pointsWithDistance.reduce(min));

    if (_markerType == 1) stopPoint += _outgoing!.length;

    return stopPoint;
  }

  static double _getDistance(toolkit.LatLng latLng, toolkit.LatLng latLng1) {
    return Geolocator.distanceBetween(
        latLng.latitude, latLng.longitude, latLng1.latitude, latLng1.longitude);
  }

  static List<toolkit.LatLng> getActiveDrivers() {
    List<toolkit.LatLng> activeDrivers = _context
        .read<DriversLocations>()
        .markers
        .map((marker) =>
            toolkit.LatLng(marker.position.latitude, marker.position.longitude))
        .toList();

    Position? userPosition = _context.read<UserLocation>().userLocation;
    if (isUserADriver && userPosition != null) {
      activeDrivers
          .add(toolkit.LatLng(userPosition.latitude, userPosition.longitude));
    }

    return activeDrivers;
  }

  static int _isSelectedMarkerAStop(Marker marker) {
    String markerIdValue = marker.markerId.value;

    if (markerIdValue.startsWith('Stop_l') ||
        markerIdValue.startsWith('Stop_s')) {
      return 0;
    } else if (markerIdValue.startsWith('StopReturn_l') ||
        markerIdValue.startsWith('StopReturn_s')) {
      return 1;
    }

    return -1;
  }

  static int _getNearestDriver(int stopPoint, List<int> driversPoints) {
    int distance = (driversPoints[0] - stopPoint).abs();
    int idx = 0;
    for (int c = 1; c < driversPoints.length; c++) {
      int cdistance = (driversPoints[c] - stopPoint).abs();
      if (cdistance < distance) {
        idx = c;
        distance = cdistance;
      }
    }

    if (driversPoints[idx] > stopPoint && idx != 0) {
      return driversPoints[idx - 1];
    } else if (driversPoints[idx] > stopPoint &&
        idx < driversPoints.length - 1) {
      return driversPoints[idx + 1];
    }
    return driversPoints[idx];
  }

  static String _getFormattedDistance(int distance) {
    if (distance < 0) distance = 0;
    if (distance < 1000) {
      return distance.toString() + ' ' + translate('main_activity.meter');
    }
    return (((distance / 1000.0) * 10).floor() / 10).toString() +
        ' ' +
        translate('main_activity.kilometer');
  }

  static String _getEstimatedTime(int distance) {
    double penaltyMultiplier = 1.0;
    if (ServerStatus.isPeak) penaltyMultiplier += 0.5;

    if (ServerStatus.weather == Weather.nominal) {
      penaltyMultiplier += 0.3;
    } else if (ServerStatus.weather == Weather.bad) {
      penaltyMultiplier += 0.7;
    }

    distance = (distance * penaltyMultiplier).round();

    int time = ((distance * 1.0) / (_getSpeed() / 3.6)).round();

    List<int> formattedTime = _splitToComponentTimes(time);

    String hr = translate('main_activity.hour'),
        min = translate('main_activity.minute');

    if (formattedTime[0] > 1) hr = translate('main_activity.hours');
    if (formattedTime[0] >= 3 && formattedTime[0] <= 10) {
      hr = translate('main_activity.hours_3-10');
    }

    if (formattedTime[1] > 1) min = translate('main_activity.minutes');
    if (formattedTime[1] >= 3 && formattedTime[1] <= 10) {
      min = translate('main_activity.minute_3-10');
    }

    if (formattedTime[0] != 0) {
      return formattedTime[0].toString() +
          ' ' +
          hr +
          ' : ' +
          formattedTime[1].toString() +
          ' ' +
          min;
    }
    return formattedTime[1].toString() + ' ' + min;
  }

  static List<int> _splitToComponentTimes(int time) {
    int hours = (time / 3600).truncate();
    int remainder = (time - hours * 3600).truncate();
    int mins = (remainder / 60).truncate();
    if (mins == 0 && hours == 0) mins = 1;

    List<int> ints = {hours, mins}.toList();
    return ints;
  }

  static double _getSpeed() {
    int pathIndex;

    if (_context.read<MapProperties>().isAltRouteSelected) {
      pathIndex = 2;
    } else {
      pathIndex = 0;
    }

    return _selectedRoute.paths[pathIndex].averageSpeed *
            _outgoingDistancePercentageCovered +
        _selectedRoute.paths[pathIndex + 1].averageSpeed *
            _incomingDistancePercentageCovered;
  }

  static String _getFormattedSpeed() {
    return '${_getSpeed().round().toString()} ${translate('main_activity.kilometer_per_hour')}';
  }

  static PathTypes? getDriverCurrentPath(LatLng coordinates,
      LinkedHashSet<Polyline> polylines, BuildContext context) {
    if (_outgoing == null) {
      _outgoing = polylines.first.points
          .map((point) => toolkit.LatLng(point.latitude, point.longitude))
          .toList();
      _incoming = polylines.last.points
          .map((point) => toolkit.LatLng(point.latitude, point.longitude))
          .toList();
    }

    int driverSide = isDriverOnDepartureSide(
        toolkit.LatLng(coordinates.latitude, coordinates.longitude), context);

    if (driverSide == -1) {
      return null;
    }

    if (context.read<MapProperties>().isAltRouteSelected) {
      if (driverSide == 1) {
        return PathTypes.outgoingAlternative;
      } else {
        return PathTypes.incomingAlternative;
      }
    } else {
      if (driverSide == 1) {
        return PathTypes.outgoing;
      } else {
        return PathTypes.incoming;
      }
    }
  }

  static double getSpeed(
      LatLng oldLocation, LatLng location, int durationInSeconds) {
    double distance = Geolocator.distanceBetween(oldLocation.latitude,
        oldLocation.longitude, location.latitude, location.longitude);

    return distance / durationInSeconds;
  }
}

class EstimationData {
  String distance = '';
  String duration = '';
  String speed = '';

  EstimationData(this.distance, this.duration, this.speed);
}
