import 'dart:convert';

import 'package:bus_driver/Assets/Models/server_status.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:http/http.dart' as http;

class RoleBasedHttpHandler {
  static const secureStorage = FlutterSecureStorage();

  static Future<http.Response> handleRoleBasedHttpGetRequest(Uri uri,
      [bool isHead = true]) async {
    try {
      final response = await http.get(uri, headers: {
        'Content-Type': 'application/json',
        'Authorization':
            'Bearer ${(await secureStorage.read(key: 'JwtToken'))}',
      }).timeout(Duration(seconds: ServerStatus.timeout));

      if (response.statusCode != 401 || !isHead) {
        return response;
      } else {
        http.Response refreshTokenRequest = await _refreshToken();
        if (refreshTokenRequest.statusCode == 200) {
          var body = json.decode(refreshTokenRequest.body);
          await secureStorage.write(key: 'JwtToken', value: body['jwtToken']);
          await secureStorage.write(
              key: 'RefreshToken', value: body['refreshToken']);
          return handleRoleBasedHttpGetRequest(uri, false);
        } else {
          http.Response loginRequestRequest = await _login();
          if (loginRequestRequest.statusCode == 200) {
            var body = json.decode(loginRequestRequest.body);
            await secureStorage.write(key: 'JwtToken', value: body['jwtToken']);
            await secureStorage.write(
                key: 'RefreshToken', value: body['refreshToken']);
            await secureStorage.write(key: 'ID', value: body['id'].toString());
            await secureStorage.write(
                key: 'Name', value: body['name'].toString());
            return handleRoleBasedHttpGetRequest(uri, false);
          } else {
            return http.Response('', 500);
          }
        }
      }
    } catch (_) {
      return http.Response('', 500);
    }
  }

  static Future<http.Response> handleRoleBasedHttpPostRequest(Uri uri,
      [bool isHead = true, dynamic body]) async {
    try {
      final response = await http
          .post(uri,
              headers: {
                'Content-Type': 'application/json',
                'Authorization':
                    'Bearer ${(await secureStorage.read(key: 'JwtToken'))}',
              },
              body: body)
          .timeout(Duration(seconds: ServerStatus.timeout));

      if (response.statusCode != 401 || !isHead) {
        return response;
      } else {
        http.Response refreshTokenRequest = await _refreshToken();
        if (refreshTokenRequest.statusCode == 200) {
          var body = json.decode(refreshTokenRequest.body);
          await secureStorage.write(key: 'JwtToken', value: body['jwtToken']);
          await secureStorage.write(
              key: 'RefreshToken', value: body['refreshToken']);
          return handleRoleBasedHttpPostRequest(uri, false, body);
        } else {
          http.Response loginRequestRequest = await _login();
          if (loginRequestRequest.statusCode == 200) {
            var body = json.decode(loginRequestRequest.body);
            await secureStorage.write(key: 'JwtToken', value: body['jwtToken']);
            await secureStorage.write(
                key: 'RefreshToken', value: body['refreshToken']);
            await secureStorage.write(key: 'ID', value: body['id'].toString());
            await secureStorage.write(
                key: 'Name', value: body['name'].toString());
            return handleRoleBasedHttpPostRequest(uri, false);
          } else {
            return http.Response('', 500);
          }
        }
      }
    } catch (_) {
      return http.Response('', 500);
    }
  }

  static Future<http.Response> handleRoleBasedHttpPutRequest(Uri uri,
      [bool isHead = true, dynamic body]) async {
    try {
      final response = await http
          .put(uri,
              headers: {
                'Authorization':
                    'Bearer ${(await secureStorage.read(key: 'JwtToken'))}',
              },
              body: body)
          .timeout(Duration(seconds: ServerStatus.timeout));

      if (response.statusCode != 401 || !isHead) {
        return response;
      } else {
        http.Response refreshTokenRequest = await _refreshToken();
        if (refreshTokenRequest.statusCode == 200) {
          var body = json.decode(refreshTokenRequest.body);
          await secureStorage.write(key: 'JwtToken', value: body['jwtToken']);
          await secureStorage.write(
              key: 'RefreshToken', value: body['refreshToken']);
          return handleRoleBasedHttpPutRequest(uri, false, body);
        } else {
          http.Response loginRequestRequest = await _login();
          if (loginRequestRequest.statusCode == 200) {
            var body = json.decode(loginRequestRequest.body);
            await secureStorage.write(key: 'JwtToken', value: body['jwtToken']);
            await secureStorage.write(
                key: 'RefreshToken', value: body['refreshToken']);
            await secureStorage.write(key: 'ID', value: body['id'].toString());
            await secureStorage.write(
                key: 'Name', value: body['name'].toString());
            return handleRoleBasedHttpPutRequest(uri, false);
          } else {
            return http.Response('', 500);
          }
        }
      }
    } catch (_) {
      return http.Response('', 500);
    }
  }

  static Future<http.Response> _refreshToken() async {
    try {
      final response = await http.post(
          Uri.parse(
              '${ServerStatus.serverUrl}/api/People/authentication/refresh-token'),
          headers: {
            'Content-Type': 'application/json',
            'Cookie':
                'refreshToken=${await secureStorage.read(key: 'RefreshToken')}',
          }).timeout(Duration(seconds: ServerStatus.timeout));
      return response;
    } catch (_) {
      return http.Response('', 500);
    }
  }

  static Future<http.Response> _login() async {
    try {
      final response = await http
          .post(
            Uri.parse(
                '${ServerStatus.serverUrl}/api/People/authentication/authenticate'),
            headers: <String, String>{
              'Content-Type': 'application/json; charset=UTF-8',
            },
            body: json.encode({
              'number': await secureStorage.read(key: 'Number'),
              'password': await secureStorage.read(key: 'Password')
            }),
          )
          .timeout(Duration(seconds: ServerStatus.timeout));
      return response;
    } catch (_) {
      return http.Response('', 500);
    }
  }
}
