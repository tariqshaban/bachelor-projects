import 'dart:async';

import 'package:bus/Assets/Models/server_status.dart';
import 'package:connectivity_plus/connectivity_plus.dart';
import 'package:http/http.dart' as http;

class ConnectionChecker {
  static Future<bool> _isInternetAvailable() async {
    try {
      await http
          .get(Uri.parse(ServerStatus.connectionTestCall))
          .timeout(Duration(seconds: ServerStatus.timeout));
      return true;
    } catch (_) {
      return false;
    }
  }

  static Future<bool> _isDeviceConnected() async {
    var connectivityResult = await (Connectivity().checkConnectivity());
    return connectivityResult == ConnectivityResult.mobile ||
        connectivityResult == ConnectivityResult.wifi;
  }

  static Future<bool> isConnectionValid() async {
    return (await _isDeviceConnected() && await _isInternetAvailable());
  }
}
