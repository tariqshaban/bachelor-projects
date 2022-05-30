import '../Enums/weather.dart';

class ServerStatus {
  static const serverUrl = 'https://192.168.1.148:25640';
  static const connectionTestCall =
      'https://www.googleapis.com/oauth2/v3/userinfo?access_token=';

  static String appVersion = '';
  static bool isPeak = false;
  static Weather weather = Weather.good;
  static String imageDrawerDirectory = '';
  static int timeout = 10;
  static int driverTimeout = 15;
  static int driverLocationGetterInterval = 69;
  static int driverLocationSetterInterval = 69;
}
