import 'dart:async';
import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_animarker/widgets/animarker.dart';
import 'package:flutter_translate/flutter_translate.dart';
import 'package:geolocator/geolocator.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:http/http.dart' as http;
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import 'package:search_choices/search_choices.dart';

import 'Assets/Components/app_drawer.dart';
import 'Assets/Components/custom_dropdown_menu_item.dart';
import 'Assets/Components/snack_bar.dart';
import 'Assets/Enums/path_types.dart';
import 'Assets/Helpers/prefs.dart';
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
  DateTime currentBackPressTime = DateTime(2000);
  late GoogleMapController mapController;
  final futureMapController = Completer<GoogleMapController>();
  StreamSubscription<Position>? locationSubscription;
  Timer? driversTracker;
  MapRoute? selectedRoute;
  bool isActive = true;

  List<CustomDropdownMenuItem> routesDropdown = [];

  @override
  void initState() {
    Future(() {
      Inspection.inspect(context);
    });

    WidgetsBinding.instance!.addPostFrameCallback((_) {
      _handleRouteList();
      _silentlyHandleUserLocation();
    });

    WidgetsBinding.instance!.addObserver(this);

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
        title: Text(
          translate('app_name'),
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
          })
        ],
      ),
      body: WillPopScope(
        onWillPop: _onWillPop,
        child: SafeArea(
          child: Column(
            children: <Widget>[
              Container(
                height: 40,
                margin: const EdgeInsets.fromLTRB(5, 0, 5, 5),
                decoration: BoxDecoration(
                    border: Border.all(
                      color: Theme.of(context).primaryColor,
                      width: 2,
                    ),
                    borderRadius: const BorderRadius.all(
                      Radius.circular(50),
                    )),
                child: Row(
                  mainAxisSize: MainAxisSize.min,
                  children: <Widget>[
                    Expanded(
                      child: Material(
                        borderRadius: BorderRadius.circular(50),
                        clipBehavior: Clip.hardEdge,
                        child: Consumer<MapProperties>(
                          builder: (context, mapProperties, child) {
                            return SearchChoices.single(
                              padding: 0,
                              underline: const SizedBox(),
                              rightToLeft: getLanguage() == 'ar',
                              closeButton: Align(
                                alignment: AlignmentDirectional.centerEnd,
                                child: TextButton(
                                  onPressed: () {
                                    Navigator.of(context, rootNavigator: true)
                                        .pop();
                                  },
                                  child: Text(translate('main_activity.close'),
                                      style: TextStyle(
                                          color:
                                              Theme.of(context).primaryColor)),
                                ),
                              ),
                              items: routesDropdown,
                              hint: CustomDropdownMenuItem(
                                value: null,
                                child: Text(
                                    translate('main_activity.select_route')),
                              ),
                              searchHint: Text(
                                  translate('main_activity.select_route'),
                                  style: TextStyle(
                                      color: Theme.of(context).primaryColor)),
                              iconDisabledColor: Theme.of(context).primaryColor,
                              value: selectedRoute,
                              displayItem:
                                  (CustomDropdownMenuItem<dynamic> item,
                                      selected) {
                                return Container(
                                  margin: const EdgeInsetsDirectional.fromSTEB(
                                      5, 0, 5, 0),
                                  padding: const EdgeInsetsDirectional.fromSTEB(
                                      0, 5, 0, 5),
                                  child: Row(
                                    mainAxisAlignment:
                                        MainAxisAlignment.spaceBetween,
                                    children: [
                                      Flexible(
                                        child: Text((item.value as MapRoute)
                                            .getTranslatedName(context)),
                                      ),
                                      Chip(
                                        avatar: const Icon(Icons.remove_red_eye,
                                            size: 15, color: Colors.white),
                                        label: Text(
                                          NumberFormat.compact().format(
                                              (item.value as MapRoute).views),
                                          style: const TextStyle(
                                            color: Colors.white,
                                          ),
                                        ),
                                        backgroundColor:
                                            Theme.of(context).primaryColor,
                                        elevation: 6,
                                        shadowColor: Colors.grey[60],
                                        padding: const EdgeInsets.all(8),
                                      )
                                    ],
                                  ),
                                );
                              },
                              searchFn: (String? keyword,
                                  List<CustomDropdownMenuItem<dynamic>>?
                                      items) {
                                Set<int> ret = {};
                                if (keyword != null &&
                                    items != null &&
                                    keyword.isNotEmpty) {
                                  keyword.split(" ").forEach((k) {
                                    int i = 0;
                                    for (var item in items) {
                                      if (k.isNotEmpty &&
                                          ((item.value as MapRoute)
                                              .getTranslatedName(context)
                                              .toLowerCase()
                                              .contains(k.toLowerCase()))) {
                                        ret.add(i);
                                      }
                                      i++;
                                    }
                                  });
                                }
                                if (keyword!.isEmpty) {
                                  ret = Iterable<int>.generate(items!.length)
                                      .toSet();
                                }
                                return ret.toList();
                              },
                              displayClearIcon: false,
                              searchInputDecoration: const InputDecoration(),
                              menuBackgroundColor:
                                  Theme.of(context).scaffoldBackgroundColor,
                              onChanged: (MapRoute value) async {
                                if (mapProperties.selectedRoute == null ||
                                    value.id !=
                                        mapProperties.selectedRoute!.id) {
                                  mapProperties.isAltRouteSelected = false;
                                  _changeRoute(value);
                                }
                              },
                              isExpanded: true,
                            );
                          },
                        ),
                      ),
                    ),
                    Consumer<MapProperties>(
                      builder: (context, mapProperties, child) {
                        return Material(
                          borderRadius: BorderRadius.circular(50),
                          clipBehavior: Clip.hardEdge,
                          child: IconButton(
                            padding: EdgeInsets.zero,
                            icon: Icon(
                                mapProperties.selectedRoute == null ||
                                        mapProperties.selectedRoute?.id !=
                                            getFavouriteRoute()
                                    ? Icons.star_border
                                    : Icons.star,
                                color: Theme.of(context).primaryColor),
                            iconSize: 25,
                            onPressed: () {
                              if (mapProperties.selectedRoute != null) {
                                if (getFavouriteRoute() ==
                                    mapProperties.selectedRoute!.id) {
                                  mapProperties.favouriteRouteChanged();
                                  updateFavouriteRoutePref(-1);
                                  SnackBars.showTextSnackBar(
                                      context,
                                      translate(
                                          'main_activity.removed_favourite_route'));
                                } else {
                                  mapProperties.favouriteRouteChanged();
                                  updateFavouriteRoutePref(
                                      mapProperties.selectedRoute!.id);
                                  SnackBars.showTextSnackBar(
                                      context,
                                      translate('main_activity.added_route') +
                                          ' "' +
                                          mapProperties.selectedRoute!
                                              .getTranslatedName(context) +
                                          '" ' +
                                          translate(
                                              'main_activity.as_favourite_route'));
                                }
                              }
                            },
                          ),
                        );
                      },
                    ),
                  ],
                ),
              ),
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

                                        List<MapRoute> favouriteRoute =
                                            mapProperties.routes
                                                .where((mapRoute) =>
                                                    mapRoute.id ==
                                                    getFavouriteRoute())
                                                .toList();

                                        if (favouriteRoute.isNotEmpty) {
                                          _changeRoute(favouriteRoute.first);
                                        }
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

  _silentlyHandleUserLocation() async {
    LocationPermission permission = await Geolocator.checkPermission();

    if (permission == LocationPermission.whileInUse ||
        permission == LocationPermission.always) {
      await _requestLocationUpdate();
      _startLocationSubscription();
    }
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
        Geolocator.getPositionStream().listen((Position position) {
      context.read<UserLocation>().setUserLocation(position, mapController);
      if (context.read<UserLocation>().isTracking) {
        _animateToUserLocation(changeZoom: false);
      }
    });
  }

  _handleRouteList() async {
    try {
      final response = await http
          .get(Uri.parse('${ServerStatus.serverUrl}/api/Routes'))
          .timeout(Duration(seconds: ServerStatus.timeout));

      if (response.statusCode == 200) {
        context.read<MapProperties>().routes = List<MapRoute>.from(json
            .decode(response.body)
            .map((model) => MapRoute.fromJson(model)));

        routesDropdown = context
            .read<MapProperties>()
            .routes
            .map<CustomDropdownMenuItem>(
              (route) => CustomDropdownMenuItem(
                value: route,
                child: Text(
                  route.getTranslatedName(context),
                ),
              ),
            )
            .toList();
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

  Future<void> _getSelectedRouteDetails() async {
    try {
      int selectedRouteId = context.read<MapProperties>().selectedRoute!.id;
      final response = await http
          .get(Uri.parse(
              '${ServerStatus.serverUrl}/api/Routes/$selectedRouteId'))
          .timeout(Duration(seconds: ServerStatus.timeout));

      if (response.statusCode == 200) {
        context.read<MapProperties>().selectedRoute =
            MapRoute.fromJson(json.decode(response.body));
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

    if (!await Geolocator.isLocationServiceEnabled()) {
      SnackBars.showTextSnackBar(
          context, translate('main_activity.loc_service_req'));
    }
  }

  Future<void> _changeRoute(MapRoute mapRoute) async {
    MapProperties mapProperties = context.read<MapProperties>();
    DriversLocations driversLocations = context.read<DriversLocations>();

    if (mapProperties.selectedRoute != mapRoute) {
      mapProperties.selectedRoute = mapRoute;
      selectedRoute = mapRoute;
      await _getSelectedRouteDetails();

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
  }

  refreshRoutesList() {
    routesDropdown = context
        .read<MapProperties>()
        .routes
        .map<CustomDropdownMenuItem>(
          (route) => CustomDropdownMenuItem(
            value: route,
            child: Text(
              route.getTranslatedName(context),
            ),
          ),
        )
        .toList();

    context.read<MapProperties>().fillMap(context, mapController);

    context.read<UserLocation>().refreshUserLocation(mapController);
  }

  changeMapTheme() async {
    if (isDarkTheme()) {
      mapController
          .setMapStyle(await rootBundle.loadString('assets/maps/dark.json'));
    } else {
      mapController.setMapStyle(null);
    }
  }

  @override
  void didChangeAppLifecycleState(AppLifecycleState state) {
    if (state == AppLifecycleState.paused) {
      isActive = false;
    } else if (state == AppLifecycleState.resumed) {
      isActive = true;
    }
  }
}
