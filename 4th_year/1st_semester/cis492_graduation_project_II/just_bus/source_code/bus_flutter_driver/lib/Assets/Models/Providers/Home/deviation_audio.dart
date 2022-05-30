import 'dart:collection';

import 'package:bus_driver/Assets/Helpers/prefs.dart';
import 'package:flutter/cupertino.dart';
import 'package:geolocator/geolocator.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:maps_toolkit/maps_toolkit.dart' as toolkit;

class DeviationAudio with ChangeNotifier {
  bool _isEnabled = getAudioState();

  bool get isEnabled => _isEnabled;

  set isEnabled(bool value) {
    _isEnabled = value;
    updateAudioState(_isEnabled);
    notifyListeners();
  }

  bool _isNotified = false;

  bool shouldPlayAudio(Position position, LinkedHashSet<Polyline> polylines) {
    if (polylines.isEmpty) {
      return false;
    }

    bool isDriverOnDepartureSide = toolkit.PolygonUtil.isLocationOnPath(
        toolkit.LatLng(position.latitude, position.longitude),
        polylines.first.points
            .map((point) => toolkit.LatLng(point.latitude, point.longitude))
            .toList(),
        false,
        tolerance: 10);

    bool isDriverOnArrivalSide = toolkit.PolygonUtil.isLocationOnPath(
        toolkit.LatLng(position.latitude, position.longitude),
        polylines.last.points
            .map((point) => toolkit.LatLng(point.latitude, point.longitude))
            .toList(),
        false,
        tolerance: 10);

    if (isDriverOnDepartureSide || isDriverOnArrivalSide) {
      _isNotified = false;
    }

    if (!_isEnabled || _isNotified) {
      return false;
    }

    if (!isDriverOnDepartureSide && !isDriverOnArrivalSide) {
      _isNotified = true;
      return true;
    }

    return false;
  }
}
