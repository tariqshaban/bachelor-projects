import 'package:bus/Assets/Helpers/prefs.dart';
import 'package:bus/Assets/Models/image_asset.dart';
import 'package:flutter/cupertino.dart';

class Vehicle {
  late ImageAsset? image;
  late String? plateNumber;
  late String manufacturer;
  late String manufacturerAr;
  late String model;
  late String modelAr;
  late String color;
  late String? secondaryColor;
  late int capacity;

  Vehicle(
      {required this.image,
      required this.plateNumber,
      required this.manufacturer,
      required this.manufacturerAr,
      required this.model,
      required this.modelAr,
      required this.color,
      required this.secondaryColor,
      required this.capacity});

  String getTranslatedManufacturer(BuildContext context) {
    if (getLanguage() == 'en') {
      return manufacturer;
    }
    return manufacturerAr;
  }

  String getTranslatedModel(BuildContext context) {
    if (getLanguage() == 'en') {
      return model;
    }
    return modelAr;
  }

  factory Vehicle.fromJson(Map<String, dynamic> parsedJson) {
    return Vehicle(
      image: parsedJson['image'] == null
          ? null
          : ImageAsset.fromJson(parsedJson['image']),
      plateNumber: parsedJson['plateNumber'],
      manufacturer: parsedJson['manufacturer'],
      manufacturerAr: parsedJson['manufacturerAr'],
      model: parsedJson['model'],
      modelAr: parsedJson['modelAr'],
      color: parsedJson['color'],
      secondaryColor: parsedJson['secondaryColor'],
      capacity: parsedJson['capacity'],
    );
  }

  Map<String, dynamic> toJson() => {
        'image': image == null ? null : image!.toJson(),
        'plateNumber': plateNumber,
        'manufacturer': manufacturer,
        'manufacturerAr': manufacturerAr,
        'model': model,
        'modelAr': modelAr,
        'color': color,
        'secondaryColor': secondaryColor,
        'capacity': capacity,
      };
}
