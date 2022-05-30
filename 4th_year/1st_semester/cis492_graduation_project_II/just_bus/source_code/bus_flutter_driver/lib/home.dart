import 'dart:async';
import 'dart:convert';

import 'package:audioplayers/audioplayers.dart';
import 'package:background_location/background_location.dart';
import 'package:bus_driver/Assets/Helpers/arrival_estimator.dart';
import 'package:bus_driver/Assets/Models/Providers/Dialogs/active_hours.dart';
import 'package:bus_driver/Assets/Models/Providers/Home/deviation_audio.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_animarker/widgets/animarker.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:flutter_translate/flutter_translate.dart';
import 'package:geolocator/geolocator.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:provider/provider.dart';
import 'package:rate_limiter/rate_limiter.dart';

import 'Assets/Components/app_drawer.dart';
import 'Assets/Components/snack_bar.dart';
import 'Assets/Enums/path_types.dart';
import 'Assets/Helpers/prefs.dart';
import 'Assets/Helpers/role_based_http_handler.dart';
import 'Assets/Models/Providers/Home/drivers_locations.dart';
import 'Assets/Models/Providers/Home/map_properties.dart';
import 'Assets/Models/Providers/Home/map_type.dart';
import 'Assets/Models/Providers/Home/user_location.dart';
import 'Assets/Models/inspection.dart';
import 'Assets/Models/map_path.dart';
import 'Assets/Models/map_route.dart';
import 'Assets/Models/server_status.dart';

final homeKey = GlobalKey<_HomeState>();

class Home extends StatefulWidget {
  const Home({Key? key}) : super(key: key);

  @override
  _HomeState createState() => _HomeState();
}

class _HomeState extends State<Home> with WidgetsBindingObserver {
  FlutterSecureStorage secureStorage = const FlutterSecureStorage();
  DateTime currentBackPressTime = DateTime(2000);
  late GoogleMapController mapController;
  final futureMapController = Completer<GoogleMapController>();
  StreamSubscription<Position>? locationSubscription;
  Timer? driversTracker;
  MapRoute? selectedRoute;
  final AudioCache audioPlayer = AudioCache();
  bool isActive = true;
  DateTime lastLocationUpdate = DateTime.now();
  late Throttle _sendDriverLocationThrottled;

  @override
  void initState() {
    Future(() {
      Inspection.inspect(context);
    });

    WidgetsBinding.instance!.addPostFrameCallback((_) async {
      await _setDesignatedDriverRouteDetails();
      if (context.read<MapProperties>().selectedRoute != null) {
        _changeRoute(context.read<MapProperties>().selectedRoute!);
      }

      _mandateUserLocation();
    });

    _sendDriverLocationThrottled = throttle(
      (position) => _sendDriverLocation(position),
      Duration(seconds: ServerStatus.driverLocationSetterInterval),
    );

    WidgetsBinding.instance!.addObserver(this);

    showForegroundService();

    super.initState();
  }

  @override
  void dispose() {
    WidgetsBinding.instance!.removeObserver(this);
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      resizeToAvoidBottomInset: false,
      drawer: const AppDrawer(),
      appBar: AppBar(
        systemOverlayStyle: SystemUiOverlayStyle(
            statusBarColor: Theme.of(context).primaryColor),
        backgroundColor: Colors.transparent,
        title: FittedBox(
          fit: BoxFit.fitWidth,
          child: Text(
            translate('app_name'),
          ),
        ),
        elevation: 0,
        actions: <Widget>[
          Consumer<MapProperties>(builder: (context, mapProperties, child) {
            return Visibility(
              visible: _doesRouteHaveAlternativeRoute(),
              child: IconButton(
                tooltip: mapProperties.isAltRouteSelected
                    ? translate('main_activity.switch_original_route')
                    : translate('main_activity.switch_alternative_route'),
                icon: const Icon(Icons.swap_horiz),
                onPressed: () {
                  mapProperties.isAltRouteSelected =
                      !mapProperties.isAltRouteSelected;
                  mapProperties.fillMap(context, mapController);
                  _animateToRoute();
                },
              ),
            );
          }),
          Consumer<MapTileType>(builder: (context, mapTileType, child) {
            return IconButton(
              tooltip: translate('main_activity.switch_map_view'),
              icon: const Icon(Icons.map),
              onPressed: () {
                mapTileType.switchMapType();
              },
            );
          }),
          PopupMenuButton(
            itemBuilder: (context) => [
              PopupMenuItem(
                padding: EdgeInsets.zero,
                child: Consumer<DeviationAudio>(
                  builder: (context, deviationAudio, child) {
                    return ListTile(
                      leading: Icon(
                        deviationAudio.isEnabled
                            ? Icons.volume_up
                            : Icons.volume_off,
                        color: Theme.of(context).primaryColor,
                      ),
                      title: Text(deviationAudio.isEnabled
                          ? translate('main_activity.audio_on')
                          : translate('main_activity.audio_off')),
                      onTap: null,
                    );
                  },
                ),
                onTap: () {
                  context.read<DeviationAudio>().isEnabled =
                      !context.read<DeviationAudio>().isEnabled;
                },
              ),
              PopupMenuItem(
                padding: EdgeInsets.zero,
                child: ListTile(
                  leading: Icon(
                    Icons.logout,
                    color: Theme.of(context).primaryColor,
                  ),
                  title: Text(translate('main_activity.logout')),
                  onTap: null,
                ),
                onTap: () {
                  _logout();
                },
              ),
            ],
          ),
        ],
      ),
      body: WillPopScope(
        onWillPop: _onWillPop,
        child: SafeArea(
          child: Column(
            children: <Widget>[
              Expanded(
                child: Stack(
                  children: [
                    Consumer<MapProperties>(
                      builder: (context, mapProperties, child) {
                        return Consumer<MapTileType>(
                            builder: (context, mapTileType, child) {
                          return Consumer<DriversLocations>(
                            builder: (context, driversLocations, child) {
                              return Consumer<UserLocation>(
                                builder: (context, userLocation, child) {
                                  return Animarker(
                                    curve: Curves.linear,
                                    useRotation: false,
                                    shouldAnimateCamera: false,
                                    duration: const Duration(seconds: 1),
                                    mapId: futureMapController.future
                                        .then<int>((value) => value.mapId),
                                    markers: {}
                                      ..addAll(userLocation.userMarker)
                                      ..addAll(driversLocations.markers),
                                    child: GoogleMap(
                                      markers: {}
                                        ..addAll(mapProperties.markers),
                                      mapToolbarEnabled: false,
                                      scrollGesturesEnabled: true,
                                      rotateGesturesEnabled: true,
                                      zoomControlsEnabled: true,
                                      mapType: mapTileType.mapType,
                                      minMaxZoomPreference:
                                          MinMaxZoomPreference(
                                              6.8,
                                              MinMaxZoomPreference
                                                  .unbounded.maxZoom),
                                      polylines: mapProperties.polylines,
                                      initialCameraPosition:
                                          mapProperties.defaultCameraPosition,
                                      cameraTargetBounds:
                                          mapProperties.cameraBounds,
                                      onMapCreated:
                                          (GoogleMapController controller) {
                                        futureMapController
                                            .complete(controller);
                                        mapController = controller;

                                        changeMapTheme();
                                      },
                                    ),
                                  );
                                },
                              );
                            },
                          );
                        });
                      },
                    ),
                    Column(
                      children: <Widget>[
                        Align(
                          alignment: AlignmentDirectional.topEnd,
                          child: Container(
                            width: 40,
                            height: 40,
                            margin: const EdgeInsetsDirectional.only(
                                top: 10, end: 10, bottom: 5),
                            child: Consumer<UserLocation>(
                              builder: (context, userLocation, child) {
                                return FloatingActionButton(
                                  heroTag: 'btn1',
                                  tooltip: translate(
                                      'main_activity.show_current_location'),
                                  onPressed: () {
                                    _animateToUserLocation();
                                  },
                                  child: Icon(Icons.my_location,
                                      color: Theme.of(context)
                                          .colorScheme
                                          .secondary),
                                  backgroundColor:
                                      Theme.of(context).primaryColor,
                                );
                              },
                            ),
                          ),
                        ),
                        Align(
                          alignment: AlignmentDirectional.topEnd,
                          child: Container(
                            width: 40,
                            height: 40,
                            margin: const EdgeInsetsDirectional.only(
                                top: 10, end: 10, bottom: 5),
                            child: Consumer<MapProperties>(
                              builder: (context, mapProperties, child) {
                                return FloatingActionButton(
                                  heroTag: 'btn2',
                                  tooltip: translate(
                                      'main_activity.show_current_route'),
                                  onPressed: () {
                                    _animateToRoute();
                                  },
                                  child: Icon(Icons.alt_route,
                                      color: Theme.of(context)
                                          .colorScheme
                                          .secondary),
                                  backgroundColor:
                                      mapProperties.selectedRoute != null
                                          ? Theme.of(context).primaryColor
                                          : Theme.of(context)
                                              .primaryColor
                                              .withOpacity(0.4),
                                );
                              },
                            ),
                          ),
                        ),
                        Align(
                          alignment: AlignmentDirectional.topEnd,
                          child: Container(
                            width: 40,
                            height: 40,
                            margin: const EdgeInsetsDirectional.only(
                                top: 10, end: 10, bottom: 5),
                            child: Consumer<UserLocation>(
                              builder: (context, userLocation, child) {
                                return FloatingActionButton(
                                  heroTag: 'btn3',
                                  tooltip:
                                      translate('main_activity.track_location'),
                                  onPressed: () {
                                    if (userLocation.userLocation != null) {
                                      userLocation.isTracking =
                                          !userLocation.isTracking;
                                    }
                                  },
                                  child: Container(
                                    decoration: BoxDecoration(
                                      color: Colors.transparent,
                                      borderRadius: const BorderRadius.all(
                                        Radius.circular(100),
                                      ),
                                      boxShadow: userLocation.isTracking
                                          ? [
                                              BoxShadow(
                                                color: Theme.of(context)
                                                    .primaryColor,
                                                spreadRadius: 10,
                                                blurRadius: 10,
                                              )
                                            ]
                                          : [],
                                    ),
                                    child: Icon(
                                      Icons.satellite,
                                      color: Theme.of(context)
                                          .colorScheme
                                          .secondary,
                                    ),
                                  ),
                                  backgroundColor:
                                      userLocation.userLocation != null
                                          ? Theme.of(context).primaryColor
                                          : Theme.of(context)
                                              .primaryColor
                                              .withOpacity(0.4),
                                );
                              },
                            ),
                          ),
                        ),
                      ],
                    ),
                    Align(
                      alignment: AlignmentDirectional.topStart,
                      child: Consumer<ActiveHours>(
                        builder: (context, activeHours, child) {
                          return activeHours.isDuringWorkHours
                              ? Container()
                              : Tooltip(
                                  message: translate(
                                      'main_activity.outside_working_hours_tooltip'),
                                  child: Container(
                                    margin: const EdgeInsetsDirectional.only(
                                        start: 10, top: 10),
                                    padding: const EdgeInsets.all(5),
                                    decoration: BoxDecoration(
                                      borderRadius: BorderRadius.circular(8),
                                      color: Theme.of(context).primaryColor,
                                    ),
                                    child: Text(
                                      translate(
                                          'main_activity.outside_working_hours'),
                                      style:
                                          const TextStyle(color: Colors.white),
                                    ),
                                  ),
                                );
                        },
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  Future<bool> _onWillPop() {
    DateTime now = DateTime.now();
    if (now.difference(currentBackPressTime) > const Duration(seconds: 2)) {
      currentBackPressTime = now;
      SnackBars.showTextSnackBar(
          context, translate('main_activity.press_back_exit'));
      return Future.value(false);
    }
    return Future.value(true);
  }

  _handleUserLocation() async {
    await _requestLocationUpdate();

    LocationPermission permission = await Geolocator.checkPermission();

    if (permission == LocationPermission.whileInUse ||
        permission == LocationPermission.always) {
      _startLocationSubscription();
    }
  }

  _mandateUserLocation() async {
    Timer.periodic(const Duration(seconds: 5), (Timer t) async {
      await _handleUserLocation();
      if (await Geolocator.checkPermission() != LocationPermission.denied) {
        t.cancel();
        return;
      }
    });
  }

  Future<Position?> _requestLocationUpdate() async {
    LocationPermission permission;

    permission = await Geolocator.checkPermission();
    if (permission == LocationPermission.denied) {
      permission = await Geolocator.requestPermission();
    }

    if (permission == LocationPermission.deniedForever) {
      SnackBars.showTextSnackBar(
          context, translate('main_activity.loc_service_req_denied_forever'));
    }

    if (permission == LocationPermission.whileInUse ||
        permission == LocationPermission.always) {
      return await Geolocator.getLastKnownPosition();
    }

    return null;
  }

  _startLocationSubscription() {
    locationSubscription ??=
        Geolocator.getPositionStream().listen((Position position) async {
      if (context.read<ActiveHours>().isDuringWorkHours) {
        _sendDriverLocationThrottled([position]);
      }
      _checkForDeviation(position);
      context
          .read<UserLocation>()
          .setUserLocation(position, mapController, context);

      if (context.read<UserLocation>().isTracking) {
        _animateToUserLocation(changeZoom: false);
      }
    });
  }

  _doesRouteHaveAlternativeRoute() {
    MapRoute? selectedRoute = context.read<MapProperties>().selectedRoute;
    if (selectedRoute == null) {
      return false;
    }
    for (MapPath mapPath in selectedRoute.paths) {
      if (mapPath.pathType == PathTypes.outgoingAlternative) {
        return true;
      }
    }
    return false;
  }

  void _animateToRoute() {
    MapProperties mapProperties = context.read<MapProperties>();

    if (mapProperties.markers.isEmpty) {
      return;
    }

    Marker start = mapProperties.markers.elementAt(0);
    Marker end = mapProperties.markers.elementAt(1);

    if (mapProperties.selectedRoute != null) {
      LatLng southwest = (start.position.latitude < end.position.latitude)
          ? start.position
          : end.position;

      LatLng northeast = (start.position.latitude > end.position.latitude)
          ? start.position
          : end.position;

      LatLngBounds bound =
          LatLngBounds(southwest: southwest, northeast: northeast);
      mapController.animateCamera(CameraUpdate.newLatLngBounds(bound, 60));
    }
  }

  Future<void> _animateToUserLocation({bool changeZoom = true}) async {
    UserLocation userLocation = context.read<UserLocation>();

    _handleUserLocation();

    if (userLocation.userLocation != null) {
      if (changeZoom) {
        mapController.animateCamera(
          CameraUpdate.newCameraPosition(
            CameraPosition(target: userLocation.userLatLng!, zoom: 15),
          ),
        );
      } else {
        mapController.animateCamera(
          CameraUpdate.newCameraPosition(
            CameraPosition(
                target: userLocation.userLatLng!,
                zoom: await mapController.getZoomLevel()),
          ),
        );
      }
    }
  }

  Future<void> _changeRoute(MapRoute mapRoute) async {
    MapProperties mapProperties = context.read<MapProperties>();
    DriversLocations driversLocations = context.read<DriversLocations>();

    mapProperties.selectedRoute = mapRoute;
    selectedRoute = mapRoute;

    if (driversTracker != null) {
      driversTracker!.cancel();
    }

    driversTracker = Timer.periodic(
        Duration(seconds: ServerStatus.driverLocationGetterInterval),
        (Timer t) async {
      if (isActive) {
        await driversLocations.fillDrivers(
            context, mapController, mapProperties.selectedRoute!.id);
      }
    });

    mapProperties.fillMap(context, mapController);
    _animateToRoute();
  }

  refreshRoutesList() {
    context.read<MapProperties>().fillMap(context, mapController);
    context.read<UserLocation>().refreshUserLocation(mapController, context);
  }

  changeMapTheme() async {
    if (isDarkTheme()) {
      mapController
          .setMapStyle(await rootBundle.loadString('assets/maps/dark.json'));
    } else {
      mapController.setMapStyle(null);
    }
  }

  _setDesignatedDriverRouteDetails() async {
    try {
      int userId = int.parse((await secureStorage.read(key: 'ID'))!);
      final response = await RoleBasedHttpHandler.handleRoleBasedHttpGetRequest(
              Uri.parse('${ServerStatus.serverUrl}/api/Routes/utils/$userId'))
          .timeout(Duration(seconds: ServerStatus.driverTimeout));

      if (response.statusCode == 200) {
        context.read<MapProperties>().selectedRoute =
            MapRoute.fromJson(json.decode(response.body));
      } else if (response.statusCode == 429) {
        SnackBars.showTextSnackBar(
            context, translate('api_response.too_many_requests'));
      } else {
        SnackBars.showTextSnackBar(
            context, translate('api_response.server_error'));
        _logout();
      }
    } catch (_) {
      SnackBars.showTextSnackBar(context, translate('api_response.timed_out'));
    }
  }

  Future<void> _logout() async {
    if (locationSubscription != null) {
      locationSubscription!.cancel();
    }
    if (driversTracker != null) {
      driversTracker!.cancel();
    }
    await BackgroundLocation.stopLocationService();
    context.read<MapProperties>().clear();
    secureStorage.deleteAll();
    Navigator.of(context)
        .pushNamedAndRemoveUntil('/', (Route<dynamic> route) => false);
  }

  Future<void> _sendDriverLocation(Position position) async {
    LatLng oldLocation;
    LatLng location = LatLng(position.latitude, position.longitude);

    if (context.read<UserLocation>().userLatLng == null) {
      oldLocation = location;
    } else {
      oldLocation = context.read<UserLocation>().userLatLng!;
    }

    if (context.read<UserLocation>().userLatLng == null) {
      return;
    }

    LatLng userLocation = context.read<UserLocation>().userLatLng!;

    if (context.read<MapProperties>().polylines.isEmpty) {
      lastLocationUpdate = DateTime.now();
      return;
    }

    double speed = ArrivalEstimator.getSpeed(oldLocation, location,
        DateTime.now().difference(lastLocationUpdate).inSeconds);
    PathTypes? pathType = ArrivalEstimator.getDriverCurrentPath(
        userLocation, context.read<MapProperties>().polylines, context);

    if (pathType == null) {
      speed = -1;
      pathType = PathTypes.outgoing;
    }

    lastLocationUpdate = DateTime.now();

    try {
      final response = await RoleBasedHttpHandler.handleRoleBasedHttpPutRequest(
          Uri.parse(
              '${ServerStatus.serverUrl}/api/Drivers/utils/${await secureStorage.read(key: 'ID')}?latitude=${userLocation.latitude}&longitude=${userLocation.longitude}&estimatedSpeed=$speed&pathType=${PathTypes.values.indexOf(pathType) + 1}'));

      if (response.statusCode == 401) {
        SnackBars.showTextSnackBar(
            context, translate('api_response.server_error'));
        _logout();
      }
    } catch (_) {}
  }

  void _checkForDeviation(Position position) {
    if (context
        .read<DeviationAudio>()
        .shouldPlayAudio(position, context.read<MapProperties>().polylines)) {
      audioPlayer.play('audio/warning.wav');
    }
  }

  Future<void> showForegroundService() async {
    await BackgroundLocation.stopLocationService();
    await BackgroundLocation.setAndroidNotification(
        title: translate('foreground_service.title'),
        message: translate('foreground_service.message'),
        icon: '@drawable/ic_launcher_notification');
    await BackgroundLocation.startLocationService();
  }

  @override
  Future<void> didChangeAppLifecycleState(AppLifecycleState state) async {
    if (state == AppLifecycleState.paused) {
      isActive = false;
    } else if (state == AppLifecycleState.resumed) {
      isActive = true;
    }
  }
}
