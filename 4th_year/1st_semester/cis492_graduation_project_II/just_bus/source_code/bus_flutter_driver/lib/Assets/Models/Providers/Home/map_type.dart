import 'package:flutter/cupertino.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';

class MapTileType with ChangeNotifier {
  MapType _mapType = MapType.normal;

  MapType get mapType => _mapType;

  void switchMapType() {
    if (_mapType == MapType.normal) {
      _mapType = MapType.satellite;
    } else {
      _mapType = MapType.normal;
    }

    notifyListeners();
  }
}
