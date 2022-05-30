import 'package:bus_driver/Assets/Helpers/prefs.dart';
import 'package:flutter/cupertino.dart';

class Person {
  late String name;
  late String nameAr;

  Person({required this.name, required this.nameAr});

  String getTranslatedName(BuildContext context) {
    if (getLanguage() == 'en') {
      return name;
    }
    return nameAr;
  }

  factory Person.fromJson(Map<String, dynamic> parsedJson) {
    return Person(
      name: parsedJson['name'],
      nameAr: parsedJson['nameAr'],
    );
  }

  Map<String, dynamic> toJson() => {
        'name': name,
        'nameAr': nameAr,
      };
}
