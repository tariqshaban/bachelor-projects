import 'package:bus/Assets/Enums/path_types.dart';
import 'package:bus/Assets/Helpers/prefs.dart';
import 'package:bus/Assets/Models/image_asset.dart';
import 'package:flutter/cupertino.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';

import 'map_stop.dart';

class MapPath {
  late int routeId;
  late PathTypes pathType;
  late ImageAsset? image;
  late String startName;
  late String startNameAr;
  late String endName;
  late String endNameAr;
  late String path1;
  late bool isCircular;
  late double averageSpeed;
  List<MapStop> stops = [];

  MapPath(
      {required this.routeId,
      required this.pathType,
      required this.startName,
      required this.startNameAr,
      required this.endName,
      required this.endNameAr,
      required this.path1,
      required this.isCircular,
      required this.averageSpeed,
      required this.stops,
      required this.image});

  String getTranslatedStartName(BuildContext context) {
    if (getLanguage() == 'en') {
      return startName;
    }
    return startNameAr;
  }

  String getTranslatedEndName(BuildContext context) {
    if (getLanguage() == 'en') {
      return endName;
    }
    return endNameAr;
  }

  LatLng getStartCoordinates() {
    String startCoordinate = path1.split(',').first;
    return LatLng(
        double.parse(
            startCoordinate.substring(0, startCoordinate.indexOf(' '))),
        double.parse(
            startCoordinate.substring(startCoordinate.indexOf(' ') + 1)));
  }

  LatLng getEndCoordinates() {
    String startCoordinate = path1.split(',').last;
    return LatLng(
        double.parse(
            startCoordinate.substring(0, startCoordinate.indexOf(' '))),
        double.parse(
            startCoordinate.substring(startCoordinate.indexOf(' ') + 1)));
  }

  factory MapPath.fromJson(Map<String, dynamic> parsedJson) {
    return MapPath(
      routeId: parsedJson['routeId'],
      pathType: PathTypes.values[parsedJson['type'] - 1],
      image: parsedJson['image'] == null
          ? null
          : ImageAsset.fromJson(parsedJson['image']),
      startName: parsedJson['startName'].toString(),
      startNameAr: parsedJson['startNameAr'].toString(),
      endName: parsedJson['endName'].toString(),
      endNameAr: parsedJson['endNameAr'].toString(),
      path1: parsedJson['path1'],
      isCircular: parsedJson['isCircular'],
      averageSpeed: parsedJson['averageSpeed'],
      stops: List<MapStop>.from(
          parsedJson['stops'].map((x) => MapStop.fromJson(x))),
    );
  }

  Map<String, dynamic> toJson() => {
        "routeId": routeId,
        "type": pathType.index + 1,
        'image': image == null ? null : image!.toJson(),
        "startName": startName,
        "startNameAr": startNameAr,
        "endName": endName,
        "endNameAr": endNameAr,
        "path1": path1,
        "isCircular": isCircular,
        "averageSpeed": averageSpeed,
        "stops": List<dynamic>.from(stops.map((x) => x.toJson())),
      };
}
