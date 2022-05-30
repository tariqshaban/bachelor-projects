import 'dart:async';
import 'dart:convert';

import 'package:bus_driver/Assets/Models/Providers/Login/button_opacity.dart';
import 'package:flutter/material.dart';
import 'package:flutter_login/flutter_login.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:flutter_translate/flutter_translate.dart';
import 'package:http/http.dart' as http;
import 'package:provider/provider.dart';

import 'Assets/Helpers/language.dart';
import 'Assets/Helpers/prefs.dart';
import 'Assets/Models/inspection.dart';
import 'Assets/Models/server_status.dart';

class Login extends StatefulWidget {
  const Login({Key? key}) : super(key: key);

  @override
  _LoginState createState() => _LoginState();
}

class _LoginState extends State<Login> {
  static const secureStorage = FlutterSecureStorage();

  @override
  void initState() {
    Future(() {
      Inspection.inspect(context);
    });

    Future.delayed(const Duration(milliseconds: 300), () {
      context.read<ButtonOpacity>().opacity = 1;
    });
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Stack(
        children: [
          FlutterLogin(
            title: translate('app_name').toUpperCase(),
            hideForgotPasswordButton: true,
            hideProvidersTitle: true,
            userType: LoginUserType.phone,
            theme: LoginTheme(
              titleStyle: const TextStyle(fontSize: 32, color: Colors.white),
              primaryColor: Theme.of(context).primaryColor,
              textFieldStyle: TextStyle(
                color: isDarkTheme() ? Colors.white : Colors.black,
              ),
              buttonTheme: LoginButtonTheme(
                splashColor: Theme.of(context).colorScheme.secondary,
              ),
              accentColor: Theme.of(context).primaryColor,
            ),
            messages: LoginMessages(
              userHint: translate('login.number'),
              passwordHint: translate('login.password'),
              loginButton: translate('login.login'),
              forgotPasswordButton: '',
            ),
            userValidator: (value) {
              if (value!.isEmpty) {
                return translate('login.err_number_empty');
              }
              return null;
            },
            passwordValidator: (value) {
              if (value!.isEmpty) {
                return translate('login.err_password_empty');
              }
              return null;
            },
            onRecoverPassword: (_) => null,
            onLogin: (loginData) async {
              return await _login(loginData);
            },
            onSignup: null,
            onSubmitAnimationCompleted: () {
              Future.delayed(const Duration(milliseconds: 200), () {
                Navigator.of(context).pushNamedAndRemoveUntil(
                    '/home', (Route<dynamic> route) => false);
              });
            },
          ),
          Positioned.directional(
            textDirection: Directionality.of(context),
            start: 5,
            top: 30,
            child: Consumer<ButtonOpacity>(
              builder: (context, mapProperties, child) {
                return AnimatedOpacity(
                  opacity: mapProperties.opacity,
                  duration: const Duration(milliseconds: 1000),
                  child: RawMaterialButton(
                    child: Icon(Icons.language,
                        color: Theme.of(context).primaryColor),
                    fillColor: Colors.white,
                    padding: const EdgeInsets.all(10),
                    constraints: const BoxConstraints(),
                    shape: const CircleBorder(),
                    onPressed: () async {
                      if (Localizations.localeOf(context).languageCode ==
                          'ar') {
                        Language.changeLanguage(context, 'en');
                      } else {
                        Language.changeLanguage(context, 'ar');
                      }
                    },
                  ),
                );
              },
            ),
          ),
        ],
      ),
    );
  }

  Future<String?> _login(LoginData loginData) async {
    try {
      final response = await http
          .post(
            Uri.parse(
                '${ServerStatus.serverUrl}/api/People/authentication/authenticate'),
            headers: <String, String>{
              'Content-Type': 'application/json; charset=UTF-8',
            },
            body: json.encode(
                {'number': loginData.name, 'password': loginData.password}),
          )
          .timeout(Duration(seconds: ServerStatus.timeout));
      if (response.statusCode == 200) {
        dynamic responseJson = json.decode(response.body);
        secureStorage.write(key: 'JwtToken', value: responseJson['jwtToken']);
        secureStorage.write(
            key: 'RefreshToken', value: responseJson['refreshToken']);
        secureStorage.write(key: 'ID', value: responseJson['id'].toString());
        secureStorage.write(
            key: 'Name', value: responseJson['name'].toString());
        secureStorage.write(key: 'Number', value: loginData.name);
        secureStorage.write(key: 'Password', value: loginData.password);

        context.read<ButtonOpacity>().opacity = 0;
      } else if (response.statusCode == 401) {
        return translate('login.invalid_login');
      } else if (response.statusCode == 429) {
        return translate('api_response.too_many_requests');
      } else {
        return translate('api_response.server_error');
      }
    } catch (_) {
      return translate('api_response.timed_out');
    }

    return null;
  }
}
