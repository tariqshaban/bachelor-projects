import 'package:bus/Assets/Models/image_asset.dart';
import 'package:bus/Assets/Models/person.dart';
import 'package:bus/Assets/Models/vehicle.dart';

class Driver {
  late ImageAsset? image;
  late double lastLatitude;
  late double lastLongitude;
  late DateTime lastLocationUpdate;
  late Person person;
  late Vehicle vehicle;
  late int markerId;

  Driver(
      {required this.image,
      required this.lastLatitude,
      required this.lastLongitude,
      required this.lastLocationUpdate,
      required this.person,
      required this.vehicle});

  factory Driver.fromJson(Map<String, dynamic> parsedJson) {
    return Driver(
      image: parsedJson['image'] == null
          ? null
          : ImageAsset.fromJson(parsedJson['image']),
      lastLatitude: parsedJson['lastLatitude'],
      lastLongitude: parsedJson['lastLongitude'],
      lastLocationUpdate:
          DateTime.parse(parsedJson['lastLocationUpdate'].toString()),
      person: Person.fromJson(parsedJson['person']),
      vehicle: Vehicle.fromJson(parsedJson['vehicle']),
    );
  }

  Map<String, dynamic> toJson() => {
        'image': image == null ? null : image!.toJson(),
        'lastLatitude': lastLatitude,
        'lastLongitude': lastLongitude,
        'lastLocationUpdate': lastLocationUpdate,
        'person': person.toJson(),
        'vehicle': vehicle.toJson(),
      };
}
