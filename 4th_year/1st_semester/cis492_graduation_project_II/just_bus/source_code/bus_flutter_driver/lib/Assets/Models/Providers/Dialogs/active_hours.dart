import 'dart:async';

import 'package:bus_driver/Assets/Helpers/prefs.dart';
import 'package:day_picker/day_picker.dart';
import 'package:flutter/material.dart';
import 'package:flutter_translate/flutter_translate.dart';
import 'package:time_range/time_range.dart';

class ActiveHours with ChangeNotifier {
  List<DayInWeek> _days = [];

  late TimeOfDay _startTime = const TimeOfDay(hour: 0, minute: 0);
  late TimeOfDay _endTime = const TimeOfDay(hour: 0, minute: 0);
  late List<bool> _daysSelected;
  late bool _isDuringWorkHours = true;

  ActiveHours() {
    calibrateDayNames();

    TimeRangeResult selectedDates = getSelectedDates();
    _startTime = selectedDates.start;
    _endTime = selectedDates.end;

    Timer.periodic(
        const Duration(seconds: 10), (Timer t) => _updateActiveHourTime());
  }

  List<DayInWeek> get days => _days;

  TimeOfDay get startTime => _startTime;

  TimeOfDay get endTime => _endTime;

  bool get isDuringWorkHours => _isDuringWorkHours;

  updateDays() {
    _daysSelected = days.map((e) => e.isSelected).toList();

    updateSelectedDays(_daysSelected);

    _updateActiveHourTime();
  }

  calibrateDayNames() {
    _daysSelected = getSelectedDays();

    _days = [
      DayInWeek(translate('active_hours_dialog.day_names.sunday'),
          isSelected: _daysSelected[0]),
      DayInWeek(translate('active_hours_dialog.day_names.monday'),
          isSelected: _daysSelected[1]),
      DayInWeek(translate('active_hours_dialog.day_names.tuesday'),
          isSelected: _daysSelected[2]),
      DayInWeek(translate('active_hours_dialog.day_names.wednesday'),
          isSelected: _daysSelected[3]),
      DayInWeek(translate('active_hours_dialog.day_names.thursday'),
          isSelected: _daysSelected[4]),
      DayInWeek(translate('active_hours_dialog.day_names.friday'),
          isSelected: _daysSelected[5]),
      DayInWeek(translate('active_hours_dialog.day_names.saturday'),
          isSelected: _daysSelected[6]),
    ];
  }

  updateTime(TimeRangeResult? rangeResult) {
    if (rangeResult == null) {
      return;
    }

    _startTime = rangeResult.start;
    _endTime = rangeResult.end;

    updateSelectedDates(TimeRangeResult(_startTime, _endTime));

    _updateActiveHourTime();
  }

  void _updateActiveHourTime() {
    bool isDuringWorkHoursNow = _checkIsDuringWorkHours();
    if (_isDuringWorkHours != isDuringWorkHoursNow) {
      notifyListeners();
    }
    _isDuringWorkHours = isDuringWorkHoursNow;
  }

  bool _checkIsDuringWorkHours() {
    DateTime now = DateTime.now();

    if (!_daysSelected[now.weekday == 7 ? 0 : now.weekday]) {
      return false;
    }

    if (_startTime.hour == 0 &&
        _startTime.minute == 0 &&
        _endTime.hour == 0 &&
        _endTime.minute == 0) {
      return true;
    }

    DateTime startDate = DateTime(
        now.year, now.month, now.day, _startTime.hour, _startTime.minute);
    DateTime endDate =
        DateTime(now.year, now.month, now.day, _endTime.hour, _endTime.minute);

    return now.isAfter(startDate) && now.isBefore(endDate);
  }
}
