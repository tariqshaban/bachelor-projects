import 'package:shared_preferences/shared_preferences.dart';

SharedPreferences? _userPrefs;

loadPrefs() async {
  _userPrefs = await SharedPreferences.getInstance();
}

SharedPreferences get prefs => _userPrefs as SharedPreferences;

updateThemePref(bool isDarkTheme) async {
  await prefs.setBool('isDarkTheme', isDarkTheme);
}

updateLanguagePref(String language) async {
  await prefs.setString('language', language);
}

updateFavouriteRoutePref(int route) async {
  await prefs.setInt('favouriteRoute', route);
}

isDarkTheme() {
  if (!prefs.containsKey('isDarkTheme')) {
    return false;
  }

  return prefs.getBool('isDarkTheme');
}

getLanguage() {
  if (!prefs.containsKey('language')) {
    return 'none';
  }

  return prefs.getString('language');
}

getFavouriteRoute() {
  if (!prefs.containsKey('favouriteRoute')) {
    return '';
  }

  return prefs.getInt('favouriteRoute');
}
