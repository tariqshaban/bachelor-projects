import 'package:flutter/cupertino.dart';

class IconVisibility with ChangeNotifier {
  bool _isWarningVisible = false;
  bool _isWifiDisconnected = false;

  get isWarningVisible {
    return _isWarningVisible;
  }

  get isWifiDisconnected {
    return _isWifiDisconnected;
  }

  void showWarning() {
    _isWarningVisible = true;
    notifyListeners();
  }

  void hideWarning() {
    _isWarningVisible = false;
    notifyListeners();
  }

  void showWifiDisconnected() {
    _isWifiDisconnected = true;
    notifyListeners();
  }

  void hideWifiDisconnected() {
    _isWifiDisconnected = false;
    notifyListeners();
  }
}
