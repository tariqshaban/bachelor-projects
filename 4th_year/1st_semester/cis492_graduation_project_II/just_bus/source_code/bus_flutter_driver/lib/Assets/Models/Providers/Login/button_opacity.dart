import 'package:flutter/cupertino.dart';

class ButtonOpacity with ChangeNotifier {
  double _opacity = 0;

  double get opacity => _opacity;

  set opacity(double value) {
    _opacity = value;
    notifyListeners();
  }
}
