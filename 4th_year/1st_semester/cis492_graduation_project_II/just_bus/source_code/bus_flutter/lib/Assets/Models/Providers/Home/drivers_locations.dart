import 'dart:collection';
import 'dart:convert';

import 'package:bus/Assets/Components/snack_bar.dart';
import 'package:bus/Assets/Helpers/arrival_estimator.dart';
import 'package:bus/Assets/Helpers/marker_icon.dart';
import 'package:bus/Assets/Helpers/paint_rectangle.dart';
import 'package:flutter/material.dart';
import 'package:flutter_translate/flutter_translate.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:http/http.dart' as http;
import 'package:maps_toolkit/maps_toolkit.dart' as toolkit;

import '../../driver.dart';
import '../../server_status.dart';

class DriversLocations with ChangeNotifier {
  List<Driver> drivers = [];
  LinkedHashSet<Marker> markers = LinkedHashSet();

  fillDrivers(
      BuildContext context, GoogleMapController mapController, int id) async {
    try {
      final response = await http
          .get(Uri.parse('${ServerStatus.serverUrl}/api/Drivers/utils/$id'))
          .timeout(Duration(seconds: ServerStatus.timeout));
      if (response.statusCode == 200) {
        drivers = List<Driver>.from(
            json.decode(response.body).map((model) => Driver.fromJson(model)));

        int oldSize = markers.length;

        markers.clear();

        for (Driver driver in drivers) {
          _addMarker(
            markers.length,
            mapController,
            LatLng(driver.lastLatitude, driver.lastLongitude),
            _getDriverBitmap(
                LatLng(driver.lastLatitude, driver.lastLongitude), context),
            context,
          );
        }

        while (markers.length < oldSize) {
          _addMarker(markers.length, mapController, const LatLng(0, 0),
              MarkerIcon.icons['Bus']!, context, true, false);
        }
      } else if (response.statusCode == 429) {
        SnackBars.showTextSnackBar(
            context, translate('api_response.too_many_requests'));
      } else {
        SnackBars.showTextSnackBar(
            context, translate('api_response.server_error'));
      }

      notifyListeners();
    } catch (_) {
      SnackBars.showTextSnackBar(context, translate('api_response.timed_out'));
    }
  }

  void _addMarker(int markerIdValue, GoogleMapController mapController,
      LatLng position, BitmapDescriptor icon, BuildContext context,
      [bool consumeTapEvents = false, bool visible = true]) {
    MarkerId markerId = MarkerId('D-${markerIdValue.toString()}');

    // A bug in the animarker caused the marker to be tapped multiple times
    // This is a quick fix in order to ignore the other taps
    // Any marker that is not under the animarker list is not affected
    bool lock = false;

    Marker marker = Marker(
      markerId: markerId,
      position: position,
      icon: icon,
      visible: visible,
      consumeTapEvents: consumeTapEvents,
      onTap: () async {
        if (!lock) {
          lock = true;
          mapController.hideMarkerInfoWindow(markerId);
          await _onMarkerClick(markerId, mapController, context);
          Future.delayed(const Duration(milliseconds: 100), () {
            mapController.showMarkerInfoWindow(markerId);
          });
        }
        Future.delayed(const Duration(milliseconds: 1), () {
          lock = false;
        });
      },
    );

    markers.add(marker);
  }

  BitmapDescriptor _getDriverBitmap(LatLng latLng, BuildContext context) {
    int driverSide = ArrivalEstimator.isDriverOnDepartureSide(
        toolkit.LatLng(latLng.latitude, latLng.longitude), context);

    if (driverSide == 1) {
      return MarkerIcon.icons['Bus']!;
    } else if (driverSide == 0) {
      return MarkerIcon.icons['Bus_return']!;
    } else {
      return MarkerIcon.icons['Bus_deviate']!;
    }
  }

  _onMarkerClick(MarkerId markerId, GoogleMapController mapController,
      BuildContext context) async {
    Future<void> sheetCallback = showModalBottomSheet(
        context: context,
        isScrollControlled: true,
        shape: const RoundedRectangleBorder(
          borderRadius: BorderRadius.only(
              topLeft: Radius.circular(20), topRight: Radius.circular(20)),
        ),
        builder: (context) =>
            _buildBottomNavigationMenu(getDriver(markerId), context));
    sheetCallback
        .then((void value) => mapController.hideMarkerInfoWindow(markerId));
  }

  Wrap _buildBottomNavigationMenu(Driver driver, BuildContext context) {
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
                      child: Image(
                        fit: BoxFit.cover,
                        image: NetworkImage(
                            '${driver.vehicle.image!.getFullPath()}.jpg'),
                      ),
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

  Driver getDriver(MarkerId markerId) {
    Marker marker =
        markers.where((marker) => marker.markerId == markerId).first;

    for (Driver driver in drivers) {
      if (driver.lastLatitude == marker.position.latitude &&
          driver.lastLongitude == marker.position.longitude) {
        return driver;
      }
    }

    return drivers.first;
  }
}
