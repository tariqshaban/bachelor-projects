import 'package:bus_driver/Assets/Enums/path_types.dart';
import 'package:bus_driver/Assets/Helpers/prefs.dart';
import 'package:bus_driver/Assets/Models/image_asset.dart';
import 'package:flutter/cupertino.dart';

class MapStop {
  late int routeId;
  late PathTypes pathType;
  late int sequence;
  late ImageAsset? image;
  String? name;
  String? nameAr;
  late double latitude;
  late double longitude;

  MapStop(
      {required this.routeId,
      required this.pathType,
      required this.sequence,
      required this.name,
      required this.nameAr,
      required this.latitude,
      required this.longitude,
      required this.image});

  String? getTranslatedName(BuildContext context) {
    if (getLanguage() == 'en') {
      return name;
    }
    return nameAr;
  }

  factory MapStop.fromJson(Map<String, dynamic> parsedJson) {
    return MapStop(
      routeId: parsedJson['routeId'],
      pathType: PathTypes.values[parsedJson['pathType'] - 1],
      sequence: parsedJson['sequence'],
      image: parsedJson['image'] == null
          ? null
          : ImageAsset.fromJson(parsedJson['image']),
      name: parsedJson['name'].toString(),
      nameAr: parsedJson['nameAr'].toString(),
      latitude: parsedJson['latitude'],
      longitude: parsedJson['longitude'],
    );
  }

  Map<String, dynamic> toJson() => {
        "routeId": routeId,
        "pathType": pathType.index + 1,
        'image': image == null ? null : image!.toJson(),
        "sequence": sequence,
        "name": name,
        "nameAr": nameAr,
        "latitude": latitude,
        "longitude": longitude,
      };
}
