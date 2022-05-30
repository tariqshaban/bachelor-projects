import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:time_range/time_range.dart';

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

updateAudioState(bool isEnabled) async {
  await prefs.setBool('audio', isEnabled);
}

updateSelectedDays(List<bool> selectedDays) async {
  await prefs.setStringList(
      'days', selectedDays.map((e) => e ? '1' : '0').toList());
}

updateSelectedDates(TimeRangeResult rangeResult) async {
  await prefs.setStringList('dates', [
    '${rangeResult.start.hour}:${rangeResult.start.minute}',
    '${rangeResult.end.hour}:${rangeResult.end.minute}'
  ]);
}

bool isDarkTheme() {
  if (!prefs.containsKey('isDarkTheme')) {
    return false;
  }

  return prefs.getBool('isDarkTheme')!;
}

String getLanguage() {
  if (!prefs.containsKey('language')) {
    return 'none';
  }

  return prefs.getString('language')!;
}

bool getAudioState() {
  if (!prefs.containsKey('audio')) {
    return true;
  }

  return prefs.getBool('audio')!;
}

List<bool> getSelectedDays() {
  if (!prefs.containsKey('days')) {
    return [true, true, true, true, true, true, true];
  }

  List<String> selectedDays = prefs.getStringList('days')!;
  return selectedDays.map((e) => e == '1' ? true : false).toList();
}

TimeRangeResult getSelectedDates() {
  if (!prefs.containsKey('dates')) {
    return TimeRangeResult(const TimeOfDay(hour: 0, minute: 0),
        const TimeOfDay(hour: 0, minute: 0));
  }

  List<String> timeRangeResult = prefs.getStringList('dates')!;

  return TimeRangeResult(
      TimeOfDay(
          hour: int.parse(timeRangeResult[0].split(":")[0]),
          minute: int.parse(timeRangeResult[0].split(":")[1])),
      TimeOfDay(
          hour: int.parse(timeRangeResult[1].split(":")[0]),
          minute: int.parse(timeRangeResult[1].split(":")[1])));
}
