import 'package:flutter/cupertino.dart';

class DrawerBackground with ChangeNotifier {
  String _networkImage = '';

  String get networkImage => _networkImage;

  set networkImage(String value) {
    _networkImage = value;
    notifyListeners();
  }
}
