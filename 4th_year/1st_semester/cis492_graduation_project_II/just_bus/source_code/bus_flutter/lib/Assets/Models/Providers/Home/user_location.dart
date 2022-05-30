import 'package:bus/Assets/Helpers/marker_icon.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter_translate/flutter_translate.dart';
import 'package:geolocator/geolocator.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';

class UserLocation with ChangeNotifier {
  Position? _userLocation;

  late Set<Marker> _userMarker = {};

  bool _isTracking = false;

  Position? get userLocation => _userLocation;

  LatLng? get userLatLng {
    if (_userLocation == null) {
      return null;
    }
    return LatLng(_userLocation!.latitude, _userLocation!.longitude);
  }

  Set<Marker> get userMarker => _userMarker;

  bool get isTracking => _isTracking;

  set isTracking(bool value) {
    _isTracking = value;
    notifyListeners();
  }

  setUserLocation(Position? userLocation, GoogleMapController mapController) {
    if (_userMarker.isEmpty ||
        userLocation != null &&
            (_userLocation == null ||
                _userLocation!.latitude != userLocation.latitude ||
                _userLocation!.longitude != userLocation.longitude)) {
      _userLocation = userLocation;

      LatLng userLoc = const LatLng(0, 0);

      if (_userLocation != null) {
        userLoc = LatLng(_userLocation!.latitude, _userLocation!.longitude);
      }

      _setMarker(
        0,
        mapController,
        userLoc,
        InfoWindow(title: translate('main_activity.your_location')),
        MarkerIcon.icons['User']!,
        _userLocation == null,
        _userLocation != null,
      );

      notifyListeners();
    }
  }

  void refreshUserLocation(GoogleMapController mapController) {
    if (_userLocation == null) {
      return;
    }

    _setMarker(
      0,
      mapController,
      LatLng(_userLocation!.latitude, _userLocation!.longitude),
      InfoWindow(title: translate('main_activity.your_location')),
      MarkerIcon.icons['User']!,
      _userLocation == null,
      _userLocation != null,
    );
  }

  void _setMarker(int markerIdValue, GoogleMapController mapController,
      LatLng position, InfoWindow infoWindow, BitmapDescriptor icon,
      [bool consumeTapEvents = false, bool visible = true]) {
    MarkerId markerId = MarkerId(markerIdValue.toString());

    Marker marker = Marker(
      markerId: markerId,
      position: position,
      infoWindow: infoWindow,
      icon: icon,
      onTap: () async {
        mapController.hideMarkerInfoWindow(markerId);
        Future.delayed(const Duration(milliseconds: 100), () {
          mapController.showMarkerInfoWindow(markerId);
        });
      },
    );
    _userMarker = {marker};
  }
}
