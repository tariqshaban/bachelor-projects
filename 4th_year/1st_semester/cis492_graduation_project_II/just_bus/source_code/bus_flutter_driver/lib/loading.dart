import 'dart:async';
import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:flutter_translate/flutter_translate.dart';
import 'package:http/http.dart' as http;
import 'package:package_info_plus/package_info_plus.dart';
import 'package:provider/provider.dart';
import 'package:rive/rive.dart';

import 'Assets/Components/app_drawer.dart';
import 'Assets/Components/snack_bar.dart';
import 'Assets/Enums/loading_status.dart';
import 'Assets/Enums/weather.dart';
import 'Assets/Helpers/connection_checker.dart';
import 'Assets/Helpers/marker_icon.dart';
import 'Assets/Models/Providers/Home/icon_visibility.dart';
import 'Assets/Models/inspection.dart';
import 'Assets/Models/server_status.dart';

class Loading extends StatefulWidget {
  const Loading({Key? key}) : super(key: key);

  @override
  _LoadingState createState() => _LoadingState();
}

class _LoadingState extends State<Loading> {
  LoadingStatus loadingStatus = LoadingStatus.loading;
  static const secureStorage = FlutterSecureStorage();
  late RiveAnimationController _riveHideStrokeController,
      _riveHideLoadingController,
      _riveErrorController,
      _riveStrokeController,
      _riveLoadingController,
      _riveBounceController,
      _riveRotateController;

  @override
  void initState() {
    WidgetsBinding.instance!.addPostFrameCallback((_) {
      MarkerIcon.assignBitmapDescriptors(context);
      _checkPrerequisites(context.read<IconVisibility>());
    });

    _riveHideStrokeController = SimpleAnimation('Hide Stroke', autoplay: false);
    _riveHideLoadingController =
        SimpleAnimation('Hide Loading', autoplay: false);
    _riveErrorController = SimpleAnimation('Error', autoplay: false);
    _riveStrokeController = SimpleAnimation('Stroke Bus');
    _riveLoadingController = SimpleAnimation('Loading');
    _riveBounceController = SimpleAnimation('Bounce Bus');
    _riveRotateController = SimpleAnimation('Rotate Wheels');

    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SafeArea(
        child: Stack(
          children: <Widget>[
            Row(
              mainAxisAlignment: MainAxisAlignment.start,
              crossAxisAlignment: CrossAxisAlignment.start,
              children: <Widget>[
                Consumer<IconVisibility>(
                  builder: (context, iconVisibility, child) {
                    return Column(
                      crossAxisAlignment: CrossAxisAlignment.end,
                      children: <Widget>[
                        Visibility(
                          visible: iconVisibility.isWarningVisible,
                          child: IconButton(
                            icon: Icon(
                              Icons.warning,
                              color: Theme.of(context).primaryColor,
                            ),
                            tooltip: ServerStatus.appVersion.isEmpty
                                ? translate('api_response.timed_out')
                                : translate('splash_page.update_latest'),
                            onPressed: () {},
                          ),
                        ),
                        Visibility(
                          visible: iconVisibility.isWifiDisconnected,
                          child: IconButton(
                            icon: Icon(
                              Icons.signal_wifi_off,
                              color: Theme.of(context).primaryColor,
                            ),
                            tooltip: translate('splash_page.internet_required'),
                            onPressed: () {},
                          ),
                        ),
                      ],
                    );
                  },
                ),
              ],
            ),
            Center(
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                crossAxisAlignment: CrossAxisAlignment.center,
                children: <Widget>[
                  SizedBox(
                    child: RiveAnimation.asset(
                      'assets/rive/bus.riv',
                      artboard: 'Bus',
                      controllers: [
                        _riveHideStrokeController,
                        _riveHideLoadingController,
                        _riveErrorController,
                        _riveStrokeController,
                        _riveLoadingController,
                        _riveBounceController,
                        _riveRotateController,
                      ],
                    ),
                    width: 150,
                    height: 150,
                  ),
                  const Padding(
                    padding: EdgeInsets.all(30),
                  ),
                  Text(translate('app_name'),
                      style: Theme.of(context).textTheme.headline3),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  Future<void> _checkPrerequisites(IconVisibility iconVisibility) async {
    if (!await ConnectionChecker.isConnectionValid()) {
      SnackBars.showTextSnackBar(
          context, translate('splash_page.internet_required'));
      iconVisibility.showWifiDisconnected();
      showRiveError();
      Timer.periodic(const Duration(milliseconds: 500), (Timer t) async {
        if (await ConnectionChecker.isConnectionValid() && t.isActive) {
          SnackBars.showTextSnackBar(
              context, translate('splash_page.back_online'));
          iconVisibility.hideWifiDisconnected();
          _callServer(iconVisibility);
          t.cancel();
          return;
        }
      });
    } else {
      _callServer(iconVisibility);
    }
  }

  _callServer(IconVisibility iconVisibility) {
    _getServerStatus().then(
      (_) async {
        if (ServerStatus.appVersion.isEmpty) {
          iconVisibility.showWarning();
          showRiveError();
        } else {
          PackageInfo packageInfo = await PackageInfo.fromPlatform();
          String version = packageInfo.version;

          if (version != ServerStatus.appVersion) {
            SnackBars.showTextSnackBar(
                context, translate('splash_page.update_latest'));
            iconVisibility.showWarning();
            showRiveError();
          } else {
            iconVisibility.hideWarning();
            Inspection.isInspectionComplete = true;
            AppDrawer.getHeaderBackground(context);

            Future.delayed(const Duration(milliseconds: 200), () async {
              if (await secureStorage.read(key: 'JwtToken') != null) {
                Navigator.of(context).pushNamedAndRemoveUntil(
                    '/home', (Route<dynamic> route) => false);
              } else {
                Navigator.of(context).pushNamedAndRemoveUntil(
                    '/login', (Route<dynamic> route) => false);
              }
            });
          }
        }
      },
    );
  }

  Future<void> _getServerStatus() async {
    try {
      final response = await http
          .get(Uri.parse('${ServerStatus.serverUrl}/api/Configurations'))
          .timeout(Duration(seconds: ServerStatus.timeout));
      if (response.statusCode == 200) {
        dynamic responseJson = json.decode(response.body)[0];
        ServerStatus.appVersion = responseJson['appVersion'];
        ServerStatus.isPeak = responseJson['isPeak'];
        ServerStatus.weather = Weather.values[responseJson['weather'] - 1];
        ServerStatus.imageDrawerDirectory =
            responseJson['imageDrawerDirectory'];
        ServerStatus.timeout = responseJson['timeout'];
        ServerStatus.driverTimeout = responseJson['driverTimeout'];
        ServerStatus.driverLocationGetterInterval =
            responseJson['driverLocationGetterInterval'];
        ServerStatus.driverLocationSetterInterval =
            responseJson['driverLocationSetterInterval'];
      } else if (response.statusCode == 429) {
        SnackBars.showTextSnackBar(
            context, translate('api_response.too_many_requests'));
      } else {
        SnackBars.showTextSnackBar(
            context, translate('api_response.server_error'));
      }
    } catch (_) {
      SnackBars.showTextSnackBar(context, translate('api_response.timed_out'));
    }
  }

  void showRiveError() {
    _riveHideLoadingController.isActive = true;
    _riveStrokeController.isActive = false;
    _riveHideStrokeController.isActive = true;
    Future.delayed(const Duration(milliseconds: 500), () {
      _riveErrorController.isActive = true;
    });
  }
}
