import 'package:bus_driver/Assets/Helpers/prefs.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

import 'map_path.dart';

class MapRoute {
  late int id;
  late String name;
  late String nameAr;
  late int views;
  List<MapPath> paths = [];

  MapRoute(
      {required this.id,
      required this.name,
      required this.nameAr,
      required this.views,
      required this.paths});

  String getTranslatedName(BuildContext context) {
    if (getLanguage() == 'en') {
      return name;
    }
    return nameAr;
  }

  factory MapRoute.fromJson(Map<String, dynamic> parsedJson) {
    return MapRoute(
      id: parsedJson['id'],
      name: parsedJson['name'],
      nameAr: parsedJson['nameAr'],
      views: parsedJson['views'],
      paths: List<MapPath>.from(
          parsedJson['paths'].map((x) => MapPath.fromJson(x))),
    );
  }

  Map<String, dynamic> toJson() => {
        'id': id,
        'name': name,
        'nameAr': nameAr,
        'views': views,
        'stops': List<dynamic>.from(paths.map((x) => x.toJson())),
      };
}
