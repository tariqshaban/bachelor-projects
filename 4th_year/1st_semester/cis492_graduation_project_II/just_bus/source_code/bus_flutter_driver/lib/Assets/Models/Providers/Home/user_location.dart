import 'package:bus_driver/Assets/Helpers/marker_icon.dart';
import 'package:bus_driver/Assets/Helpers/paint_rectangle.dart';
import 'package:bus_driver/Assets/Helpers/panorama_hotfix.dart';
import 'package:flutter/material.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:flutter_translate/flutter_translate.dart';
import 'package:geolocator/geolocator.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:provider/provider.dart';

import '../../driver.dart';
import 'drivers_locations.dart';

class UserLocation with ChangeNotifier {
  final FlutterSecureStorage _secureStorage = const FlutterSecureStorage();

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

  setUserLocation(Position? userLocation, GoogleMapController mapController,
      BuildContext context) {
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
        MarkerIcon.icons['User_driver']!,
        context,
        _userLocation == null,
        _userLocation != null,
      );

      notifyListeners();
    }
  }

  void refreshUserLocation(
      GoogleMapController mapController, BuildContext context) {
    if (_userLocation == null) {
      return;
    }

    _setMarker(
      0,
      mapController,
      LatLng(_userLocation!.latitude, _userLocation!.longitude),
      InfoWindow(title: translate('main_activity.your_location')),
      MarkerIcon.icons['User_driver']!,
      context,
      _userLocation == null,
      _userLocation != null,
    );
  }

  void _setMarker(
      int markerIdValue,
      GoogleMapController mapController,
      LatLng position,
      InfoWindow infoWindow,
      BitmapDescriptor icon,
      BuildContext context,
      [bool consumeTapEvents = false,
      bool visible = true]) {
    MarkerId markerId = MarkerId(markerIdValue.toString());

    Marker marker = Marker(
      markerId: markerId,
      position: position,
      infoWindow: infoWindow,
      icon: icon,
      onTap: () async {
        mapController.hideMarkerInfoWindow(markerId);
        await _onMarkerClick(markerId, mapController, context);
        Future.delayed(const Duration(milliseconds: 100), () {
          mapController.showMarkerInfoWindow(markerId);
        });
      },
    );
    _userMarker = {marker};
  }

  _onMarkerClick(MarkerId markerId, GoogleMapController mapController,
      BuildContext context) async {
    Driver? driver = await getDriver(markerId, context);
    if (driver == null) {
      return;
    }

    Future<void> sheetCallback = showModalBottomSheet(
        context: context,
        isScrollControlled: true,
        shape: const RoundedRectangleBorder(
          borderRadius: BorderRadius.only(
              topLeft: Radius.circular(20), topRight: Radius.circular(20)),
        ),
        builder: (context) => _buildBottomNavigationMenu(driver, context));
    sheetCallback
        .then((void value) => mapController.hideMarkerInfoWindow(markerId));
  }

  Wrap _buildBottomNavigationMenu(Driver driver, BuildContext context) {
    late Widget image;
    if (driver.vehicle.image != null) {
      if (driver.vehicle.image!.is360) {
        image = PanoramaHotfix(
          animSpeed: 4,
          interactive: false,
          child: Image.network('${driver.vehicle.image!.getFullPath()}.jpg'),
        );
      } else {
        image = Image(
          fit: BoxFit.cover,
          image: NetworkImage('${driver.vehicle.image!.getFullPath()}.jpg'),
        );
      }
    }

    return Wrap(
      children: <Widget>[
        Stack(
          children: [
            driver.vehicle.image == null
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
            driver.image == null
                ? Container()
                : Padding(
                    padding:
                        const EdgeInsetsDirectional.only(start: 10, top: 10),
                    child: CircleAvatar(
                      radius: 40,
                      backgroundImage:
                          NetworkImage('${driver.image!.getFullPath()}.jpg'),
                      backgroundColor: Colors.transparent,
                    ),
                  ),
          ],
        ),
        ListTile(
          leading: Icon(Icons.person, color: Theme.of(context).primaryColor),
          title: Text(translate('main_activity.driver') +
              '   ' +
              driver.person.getTranslatedName(context)),
        ),
        ListTile(
          leading:
              Icon(Icons.credit_card, color: Theme.of(context).primaryColor),
          title: Text(translate('main_activity.plate_number') +
              '   ' +
              (driver.vehicle.plateNumber == null
                  ? '-'
                  : driver.vehicle.plateNumber!)),
        ),
        ListTile(
          leading: Icon(Icons.business, color: Theme.of(context).primaryColor),
          title: Text(translate('main_activity.manufacturer') +
              '   ' +
              driver.vehicle.getTranslatedManufacturer(context)),
        ),
        ListTile(
          leading:
              Icon(Icons.car_repair, color: Theme.of(context).primaryColor),
          title: Text(translate('main_activity.model') +
              '   ' +
              driver.vehicle.getTranslatedModel(context)),
        ),
        ListTile(
          leading:
              Icon(Icons.color_lens, color: Theme.of(context).primaryColor),
          title: Row(children: [
            Text(translate('main_activity.paint') + '   '),
            CustomPaint(
              size: const Size(20, 20),
              painter: PaintRectangle(
                primaryColor: Color(int.parse("0xFF${driver.vehicle.color}")),
                secondaryColor: driver.vehicle.secondaryColor == null
                    ? null
                    : Color(
                        int.parse("0xFF${driver.vehicle.secondaryColor}"),
                      ),
              ),
            )
          ]),
        ),
        ListTile(
          leading:
              Icon(Icons.event_seat, color: Theme.of(context).primaryColor),
          title: Text(translate('main_activity.capacity') +
              '   ' +
              driver.vehicle.capacity.toString()),
        ),
      ],
    );
  }

  Future<Driver?> getDriver(MarkerId markerId, BuildContext context) async {
    for (Driver driver in context.read<DriversLocations>().drivers) {
      if (driver.personId.toString() == await _secureStorage.read(key: 'ID')) {
        return driver;
      }
    }

    return null;
  }
}
