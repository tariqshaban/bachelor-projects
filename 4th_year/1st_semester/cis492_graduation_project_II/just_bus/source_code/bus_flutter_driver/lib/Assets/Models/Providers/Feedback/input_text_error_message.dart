import 'package:flutter/cupertino.dart';

class InputTextErrorMessage with ChangeNotifier {
  int _errorMessage = -1;

  int get errorMessage => _errorMessage;

  set errorMessage(int value) {
    _errorMessage = value;
    notifyListeners();
  }
}
