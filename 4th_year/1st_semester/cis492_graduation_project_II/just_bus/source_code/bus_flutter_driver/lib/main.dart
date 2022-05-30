//flutter build apk --release --target-platform android-arm,android-arm64,android-x64 --split-per-abi
//flutter build apk --release lib\Splash.dart
//Remove WebDAVModule from IIS 'in modules'; it will cause post request to return 405
//LocationUpdateService.kt .setColor(0xFF0E8B8B.toInt()) line 86
//call _loadTexture(widget.child!.image) in _updateView() to ensure that the image is loaded everytime the navigation drawer is shown
import 'dart:async';
import 'dart:io';

import 'package:bus_driver/Assets/Models/Providers/Dialogs/active_hours.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:flutter_translate/flutter_translate.dart';
import 'package:provider/provider.dart';

import 'Assets/Helpers/http_override.dart';
import 'Assets/Helpers/prefs.dart';
import 'Assets/Models/Providers/AppDrawer/drawer_background.dart';
import 'Assets/Models/Providers/Dialogs/reachability.dart';
import 'Assets/Models/Providers/Feedback/checkbox_value.dart';
import 'Assets/Models/Providers/Feedback/input_text_error_message.dart';
import 'Assets/Models/Providers/Home/deviation_audio.dart';
import 'Assets/Models/Providers/Home/drivers_locations.dart';
import 'Assets/Models/Providers/Home/icon_visibility.dart';
import 'Assets/Models/Providers/Home/map_properties.dart';
import 'Assets/Models/Providers/Home/map_type.dart';
import 'Assets/Models/Providers/Home/user_location.dart';
import 'Assets/Models/Providers/Login/button_opacity.dart';
import 'Assets/ThemesHandler/bloc/theme_bloc.dart';
import 'Assets/ThemesHandler/bloc/theme_state.dart';
import 'Assets/ThemesHandler/ui/themes.dart';
import 'feedback.dart' as feedback;
import 'home.dart';
import 'loading.dart';
import 'login.dart';

Future<void> main() async {
  WidgetsFlutterBinding.ensureInitialized();
  HttpOverrides.global = HttpOverride();
  await loadPrefs();

  var delegate = await LocalizationDelegate.create(
      fallbackLocale: 'en', supportedLocales: ['en', 'ar']);

  Locale locale = const Locale('en');
  String language = getLanguage();
  if (language == 'none') {
    String systemLocale = Platform.localeName.substring(0, 2);
    locale = Locale(systemLocale);
    updateLanguagePref(systemLocale);
  } else {
    locale = Locale(language);
  }

  delegate.changeLocale(locale);
  runApp(LocalizedApp(delegate, const Root()));
}

class Root extends StatelessWidget {
  const Root({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    var localizationDelegate = LocalizedApp.of(context).delegate;

    return MultiProvider(
      providers: [
        BlocProvider<ThemeBloc>(create: (context) => ThemeBloc()),
        ChangeNotifierProvider<IconVisibility>(
            create: (context) => IconVisibility()),
        ChangeNotifierProvider<MapProperties>(
            create: (context) => MapProperties()),
        ChangeNotifierProvider<DriversLocations>(
            create: (context) => DriversLocations()),
        ChangeNotifierProvider<MapTileType>(create: (context) => MapTileType()),
        ChangeNotifierProvider<UserLocation>(
            create: (context) => UserLocation()),
        ChangeNotifierProvider<ButtonOpacity>(
            create: (context) => ButtonOpacity()),
        ChangeNotifierProvider<DeviationAudio>(
            create: (context) => DeviationAudio()),
        ChangeNotifierProvider<CheckboxValue>(
            create: (context) => CheckboxValue()),
        ChangeNotifierProvider<InputTextErrorMessage>(
            create: (context) => InputTextErrorMessage()),
        ChangeNotifierProvider<DrawerBackground>(
            create: (context) => DrawerBackground()),
        ChangeNotifierProvider<Reachability>(
            create: (context) => Reachability()),
        ChangeNotifierProvider<ActiveHours>(create: (context) => ActiveHours()),
      ],
      child:
          BlocBuilder<ThemeBloc, ThemeState>(builder: (context, _themeState) {
        return LocalizationProvider(
          state: LocalizationProvider.of(context).state,
          child: MaterialApp(
            theme: isDarkTheme() ? buildThemeTwo() : buildThemeOne(),
            localizationsDelegates: [
              GlobalMaterialLocalizations.delegate,
              GlobalWidgetsLocalizations.delegate,
              GlobalCupertinoLocalizations.delegate,
              localizationDelegate
            ],
            supportedLocales: localizationDelegate.supportedLocales,
            locale: localizationDelegate.currentLocale,
            title: translate('app_name'),
            initialRoute: '/',
            routes: {
              '/': (context) => const Loading(),
              '/login': (context) => const Login(),
              '/home': (context) => Home(key: homeKey),
              '/feedback': (context) => const feedback.Feedback(),
            },
          ),
        );
      }),
    );
  }
}
