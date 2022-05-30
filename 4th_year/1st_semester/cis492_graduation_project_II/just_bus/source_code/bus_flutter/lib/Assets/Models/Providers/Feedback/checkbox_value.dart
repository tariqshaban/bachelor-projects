import 'package:flutter/cupertino.dart';

class CheckboxValue with ChangeNotifier {
  bool _isEnabled = false;

  bool get isEnabled => _isEnabled;

  set isEnabled(bool value) {
    _isEnabled = value;
    notifyListeners();
  }
}
