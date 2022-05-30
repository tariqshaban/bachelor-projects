import 'dart:convert';

import 'package:bus/Assets/Helpers/prefs.dart';
import 'package:bus/Assets/Models/Providers/AppDrawer/drawer_background.dart';
import 'package:bus/Assets/Models/server_status.dart';
import 'package:bus/Assets/ThemesHandler/bloc/theme_bloc.dart';
import 'package:bus/Assets/ThemesHandler/bloc/theme_event.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_translate/flutter_translate.dart';
import 'package:http/http.dart' as http;
import 'package:provider/provider.dart';

import '../../home.dart';
import 'dialogs.dart';

class AppDrawer extends StatelessWidget {
  const AppDrawer({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return SafeArea(
      child: Drawer(
        child: ListView(
          children: <Widget>[
            _header(context),
            _themeSwitch(context),
            _languageDropdown(context),
            const Divider(),
            _feedbackListItem(context),
            _showReachabilityTest(context),
          ],
        ),
      ),
    );
  }

  Widget _header(BuildContext context) {
    return Consumer<DrawerBackground>(
      builder: (context, drawerBackground, child) {
        ImageProvider image;
        if (drawerBackground.networkImage.isNotEmpty) {
          image = NetworkImage(
              '${ServerStatus.imageDrawerDirectory}${drawerBackground.networkImage}');
        } else {
          image = const AssetImage('assets/images/header_background.png');
        }

        return UserAccountsDrawerHeader(
          accountName: Text(
              '${translate('app_name')} ${ServerStatus.appVersion.toString()}'),
          accountEmail: Text(_getImageCreator(drawerBackground.networkImage)),
          currentAccountPicture: const CircleAvatar(
            backgroundImage: AssetImage('assets/images/ic_launcher.png'),
            backgroundColor: Colors.transparent,
          ),
          otherAccountsPictures: const [
            CircleAvatar(
              backgroundImage: AssetImage('assets/images/just.png'),
              backgroundColor: Colors.transparent,
            ),
          ],
          decoration: BoxDecoration(
            color: Theme.of(context).colorScheme.secondary,
            image: DecorationImage(
              image: image,
              colorFilter: ColorFilter.mode(
                  Colors.black.withOpacity(0.2), BlendMode.darken),
              fit: BoxFit.cover,
            ),
          ),
        );
      },
    );
  }

  Widget _themeSwitch(BuildContext context) {
    return ListTile(
      leading:
          Icon(Icons.format_color_fill, color: Theme.of(context).primaryColor),
      title: Text(translate("nav_drawer.dark_theme")),
      trailing: Switch(
        activeColor: Theme.of(context).primaryColor,
        value: isDarkTheme(),
        onChanged: (bool value) {
          _changeTheme(context);
        },
      ),
      onTap: () {
        _changeTheme(context);
      },
    );
  }

  Widget _languageDropdown(BuildContext context) {
    final GlobalKey dropdownKey = GlobalKey();
    return ListTile(
      leading: Icon(Icons.language, color: Theme.of(context).primaryColor),
      title: Text(translate("nav_drawer.language")),
      trailing: Padding(
        padding: const EdgeInsetsDirectional.only(end: 5),
        child: DropdownButton<String>(
          key: dropdownKey,
          iconEnabledColor: Theme.of(context).primaryColor,
          underline: const SizedBox(),
          items: <String>['English', 'العربية'].map((String value) {
            return DropdownMenuItem<String>(
              value: value,
              child: Text(value),
            );
          }).toList(),
          onChanged: (value) async {
            if (getLanguage() == 'en' && value != 'English' ||
                getLanguage() == 'ar' && value != 'العربية') {
              if (value == 'English') {
                await changeLocale(context, 'en');
                updateLanguagePref('en');
                Navigator.pop(context, 'en');
                homeKey.currentState!.refreshRoutesList();
              } else {
                await changeLocale(context, 'ar');
                updateLanguagePref('ar');
                Navigator.pop(context, 'ar');
                homeKey.currentState!.refreshRoutesList();
              }
            }
          },
        ),
      ),
      onTap: () {
        _openDropdown(dropdownKey);
      },
    );
  }

  Widget _feedbackListItem(BuildContext context) {
    return ListTile(
      leading: Icon(Icons.feedback, color: Theme.of(context).primaryColor),
      title: Text(translate("nav_drawer.feedback")),
      onTap: () {
        Navigator.of(context).pushNamed('/feedback');
      },
    );
  }

  Widget _showReachabilityTest(BuildContext context) {
    return ListTile(
      leading: Icon(Icons.wifi, color: Theme.of(context).primaryColor),
      title: Text(translate('nav_drawer.connection_test')),
      onTap: () {
        Dialogs.getReachabilityTestDialog(context);
      },
    );
  }

  void _openDropdown(GlobalKey dropdownKey) {
    GestureDetector? detector;
    void searchForGestureDetector(BuildContext? element) {
      element!.visitChildElements((element) {
        if (element.widget is GestureDetector) {
          detector = element.widget as GestureDetector?;
          return;
        } else {
          searchForGestureDetector(element);
        }
        return;
      });
    }

    searchForGestureDetector(dropdownKey.currentContext);
    assert(detector != null);

    detector!.onTap!();
  }

  static Future<void> getHeaderBackground(BuildContext context) async {
    final response = await http
        .get(Uri.parse('${ServerStatus.serverUrl}/api/DrawerImages'))
        .timeout(Duration(seconds: ServerStatus.timeout));
    if (response.statusCode == 200) {
      context.read<DrawerBackground>().networkImage =
          json.decode(response.body)['image'];
    }
  }

  static String _getImageCreator(String imageName) {
    if (imageName.isEmpty) {
      return '';
    }
    return imageName.substring(0, imageName.lastIndexOf('.'));
  }

  static _changeTheme(BuildContext context) {
    BlocProvider.of<ThemeBloc>(context).add(ToggleThemeEvent());
    updateThemePref(!isDarkTheme());
    SystemChrome.setSystemUIOverlayStyle(
      SystemUiOverlayStyle(
        systemNavigationBarColor:
        isDarkTheme() ? const Color(0xff303030) : const Color(0xfffafafa),
        systemNavigationBarIconBrightness:
        isDarkTheme() ? Brightness.light : Brightness.dark,
      ),
    );
    homeKey.currentState!.changeMapTheme();
    Navigator.pop(context);
  }
}
