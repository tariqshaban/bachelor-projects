import 'dart:collection';

import 'package:bus/Assets/Enums/path_types.dart';
import 'package:bus/Assets/Helpers/arrival_estimator.dart';
import 'package:bus/Assets/Helpers/marker_icon.dart';
import 'package:bus/Assets/Helpers/panorama_hotfix.dart';
import 'package:bus/Assets/Models/image_asset.dart';
import 'package:flutter/material.dart';
import 'package:flutter_translate/flutter_translate.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';

import '../../map_path.dart';
import '../../map_route.dart';
import '../../map_stop.dart';

class MapProperties with ChangeNotifier {
  late BuildContext _context;
  static int counter = 0;

  bool _isAltRouteSelected = false;
  bool _doesRouteContainAlt = false;
  List<MapRoute> _routes = List.unmodifiable([]);

  MapRoute? _selectedRoute;

  LinkedHashSet<Marker> markers = LinkedHashSet();
  LinkedHashSet<Polyline> polylines = LinkedHashSet();

  CameraPosition defaultCameraPosition = const CameraPosition(
    target: LatLng(31.4645395, 37.022226),
    zoom: 6.8,
  );

  CameraTargetBounds cameraBounds = CameraTargetBounds(
    LatLngBounds(
      southwest: const LatLng(29.372604, 34.908549),
      northeast: const LatLng(33.556475, 39.135903),
    ),
  );

  bool get isAltRouteSelected => _isAltRouteSelected;

  bool get doesRouteContainAlt => _doesRouteContainAlt;

  MapRoute? get selectedRoute => _selectedRoute;

  List<MapRoute> get routes => _routes;

  set isAltRouteSelected(bool isAltRouteSelected) {
    _isAltRouteSelected = isAltRouteSelected;
    notifyListeners();
  }

  set doesRouteContainAlt(bool doesRouteContainAlt) {
    _doesRouteContainAlt = doesRouteContainAlt;
    notifyListeners();
  }

  set selectedRoute(MapRoute? selectedRoute) {
    _selectedRoute = selectedRoute;
    notifyListeners();
  }

  set routes(List<MapRoute> value) {
    _routes = value;
    notifyListeners();
  }

  void favouriteRouteChanged() {
    notifyListeners();
  }

  fillMap(BuildContext context, GoogleMapController mapController) {
    if (_selectedRoute == null) {
      return;
    }

    _context = context;

    polylines.clear();
    markers.clear();
    _fillPolyLines();
    _fillPathMarkers(mapController);
  }

  _fillPolyLines() {
    for (MapPath path in selectedRoute!.paths) {
      if (_shouldPathBeInvisible(path)) {
        continue;
      }

      List<LatLng> points = [];

      if ((path.pathType == PathTypes.incoming ||
              path.pathType == PathTypes.incomingAlternative) &&
          selectedRoute!
              .paths[selectedRoute!.paths.indexOf(path) - 1].isCircular) {
        points.add(polylines.last.points.last);
      }

      List<String> pathList = path.path1.split(',');
      for (String latLng in pathList) {
        points.add(LatLng(
            double.parse(latLng.substring(0, latLng.indexOf(' '))),
            double.parse(latLng.substring(latLng.indexOf(' ') + 1))));
      }

      if ((path.pathType == PathTypes.incoming ||
              path.pathType == PathTypes.incomingAlternative) &&
          path.isCircular) {
        points.add(polylines.last.points.first);
      }

      Color polyColor;
      if (path.pathType == PathTypes.outgoing ||
          path.pathType == PathTypes.outgoingAlternative) {
        polyColor = Theme.of(_context).primaryColor;
      } else {
        polyColor = Theme.of(_context).colorScheme.secondary;
      }

      polylines.add(Polyline(
          polylineId: PolylineId(polylines.length.toString()),
          visible: true,
          points: points,
          color: Colors.black,
          width: 4));
      polylines.add(Polyline(
          polylineId: PolylineId(polylines.length.toString()),
          visible: true,
          points: points,
          color: polyColor,
          width: 3));
    }
  }

  _fillPathMarkers(GoogleMapController mapController) {
    String stopType;

    for (MapPath path in selectedRoute!.paths) {
      if (_shouldPathBeInvisible(path)) {
        continue;
      }

      if (path.pathType == PathTypes.outgoing ||
          path.pathType == PathTypes.outgoingAlternative) {
        stopType = 'Stop_l';
      } else {
        stopType = 'StopReturn_l';
      }

      if (path.pathType == PathTypes.outgoing ||
          path.pathType == PathTypes.outgoingAlternative ||
          !selectedRoute!
              .paths[selectedRoute!.paths.indexOf(path) - 1].isCircular) {
        _addMarker(
            '$stopType-s',
            mapController,
            path.getStartCoordinates(),
            InfoWindow(title: path.getTranslatedStartName(_context)),
            MarkerIcon.icons[stopType]!);
      }

      if (path.pathType == PathTypes.outgoing ||
          path.pathType == PathTypes.outgoingAlternative ||
          !path.isCircular) {
        _addMarker(
            '$stopType-e',
            mapController,
            path.getEndCoordinates(),
            InfoWindow(title: path.getTranslatedEndName(_context)),
            MarkerIcon.icons[stopType]!);
      }
    }

    for (MapPath path in selectedRoute!.paths) {
      if (_shouldPathBeInvisible(path)) {
        continue;
      }

      if (path.pathType == PathTypes.outgoing ||
          path.pathType == PathTypes.outgoingAlternative) {
        stopType = 'Stop_s';
      } else {
        stopType = 'StopReturn_s';
      }

      for (MapStop stop in path.stops) {
        _addMarker(
            '$stopType-${counter++}',
            mapController,
            LatLng(stop.latitude, stop.longitude),
            InfoWindow(title: stop.getTranslatedName(_context)),
            MarkerIcon.icons[stopType]!);
      }
    }
  }

  void _addMarker(String markerIdValue, GoogleMapController mapController,
      LatLng position, InfoWindow infoWindow, BitmapDescriptor icon,
      [bool consumeTapEvents = false, bool visible = true]) {
    if (infoWindow.title == 'null') {
      infoWindow = InfoWindow.noText;
    }

    MarkerId markerId = MarkerId(markerIdValue);

    Marker marker = Marker(
      markerId: markerId,
      position: position,
      infoWindow: infoWindow,
      icon: icon,
      consumeTapEvents: consumeTapEvents,
      visible: visible,
      onTap: () async {
        mapController.hideMarkerInfoWindow(markerId);
        await _onMarkerClick(markerId, mapController);
        Future.delayed(const Duration(milliseconds: 100), () {
          mapController.showMarkerInfoWindow(markerId);
        });
      },
    );

    markers.add(marker);
  }

  bool _shouldPathBeInvisible(MapPath path) {
    return (path.pathType == PathTypes.outgoing ||
                path.pathType == PathTypes.incoming) &&
            _isAltRouteSelected ||
        (path.pathType == PathTypes.outgoingAlternative ||
                path.pathType == PathTypes.incomingAlternative) &&
            !_isAltRouteSelected;
  }

  _onMarkerClick(MarkerId markerId, GoogleMapController mapController) async {
    EstimationData estimationData = await ArrivalEstimator.getEstimation(
        selectedRoute!, _context, markerId, mapController);

    Future<void> sheetCallback = showModalBottomSheet(
        context: _context,
        isScrollControlled: true,
        shape: const RoundedRectangleBorder(
          borderRadius: BorderRadius.only(
              topLeft: Radius.circular(20), topRight: Radius.circular(20)),
        ),
        builder: (context) => estimationData.duration.isEmpty
            ? _buildErrorBottomNavigationMenu(getStopImage(markerId), context)
            : _buildBottomNavigationMenu(
                getStopImage(markerId), estimationData, context));
    sheetCallback
        .then((void value) => mapController.hideMarkerInfoWindow(markerId));
  }

  Wrap _buildBottomNavigationMenu(ImageAsset? stopImage,
      EstimationData estimationData, BuildContext context) {
    late Widget image;
    if (stopImage != null) {
      if (stopImage.is360) {
        image = PanoramaHotfix(
          animSpeed: 4,
          interactive: false,
          child: Image.network('${stopImage.getFullPath()}.jpg'),
        );
      } else {
        image = Image(
          fit: BoxFit.cover,
          image: NetworkImage('${stopImage.getFullPath()}.jpg'),
        );
      }
    }

    return Wrap(
      children: <Widget>[
        stopImage == null
            ? Container()
            : ClipRRect(
                borderRadius: const BorderRadius.only(
                    topLeft: Radius.circular(20),
                    topRight: Radius.circular(20)),
                child: ConstrainedBox(
                  constraints: BoxConstraints(
                    maxHeight: MediaQuery.of(context).size.height / 3,
                    minWidth: MediaQuery.of(context).size.width,
                  ),
                  child: image,
                ),
              ),
        ListTile(
          leading: Icon(Icons.timer, color: Theme.of(context).primaryColor),
          title: Text(translate('main_activity.duration') +
              '   ' +
              estimationData.duration),
        ),
        ListTile(
          leading: Icon(Icons.location_on_outlined,
              color: Theme.of(context).primaryColor),
          title: Text(translate('main_activity.distance') +
              '   ' +
              estimationData.distance),
        ),
        ListTile(
          leading: Icon(Icons.speed, color: Theme.of(context).primaryColor),
          title: Text(
              translate('main_activity.speed') + '   ' + estimationData.speed),
        ),
      ],
    );
  }

  Wrap _buildErrorBottomNavigationMenu(
      ImageAsset? stopImage, BuildContext context) {
    late Widget image;
    if (stopImage != null) {
      if (stopImage.is360) {
        image = PanoramaHotfix(
          animSpeed: 4,
          interactive: false,
          child: Image.network('${stopImage.getFullPath()}.jpg'),
        );
      } else {
        image = Image(
          fit: BoxFit.cover,
          image: NetworkImage('${stopImage.getFullPath()}.jpg'),
        );
      }
    }

    return Wrap(
      children: <Widget>[
        stopImage == null
            ? Container()
            : ClipRRect(
                borderRadius: const BorderRadius.only(
                    topLeft: Radius.circular(20),
                    topRight: Radius.circular(20)),
                child: ConstrainedBox(
                  constraints: BoxConstraints(
                    maxHeight: MediaQuery.of(context).size.height / 3,
                    minWidth: MediaQuery.of(context).size.width,
                  ),
                  child: image,
                ),
              ),
        ListTile(
          leading: Icon(Icons.warning, color: Theme.of(context).primaryColor),
          title: Text(translate('main_activity.no_current_drivers')),
        ),
      ],
    );
  }

  ImageAsset? getStopImage(MarkerId markerId) {
    Marker marker =
        markers.where((marker) => marker.markerId == markerId).first;

    for (MapPath path in selectedRoute!.paths) {
      LatLng pathStartPosition = getPathStartLatLng(path);
      if (pathStartPosition.latitude == marker.position.latitude &&
          pathStartPosition.longitude == marker.position.longitude) {
        return path.image;
      }
      for (MapStop stop in path.stops) {
        if (stop.latitude == marker.position.latitude &&
            stop.longitude == marker.position.longitude) {
          return stop.image;
        }
      }
    }

    return null;
  }

  LatLng getPathStartLatLng(MapPath path) {
    String firstPathPoint = path.path1.substring(0, path.path1.indexOf(','));
    return LatLng(
        double.parse(firstPathPoint.substring(0, firstPathPoint.indexOf(' '))),
        double.parse(
            firstPathPoint.substring(firstPathPoint.indexOf(' ') + 1)));
  }
}
