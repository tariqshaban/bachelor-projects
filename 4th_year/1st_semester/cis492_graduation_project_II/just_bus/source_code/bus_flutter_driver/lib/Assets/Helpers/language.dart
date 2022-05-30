import 'package:bus_driver/Assets/Helpers/prefs.dart';
import 'package:bus_driver/Assets/Models/Providers/Dialogs/active_hours.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter_translate/flutter_translate.dart';
import 'package:provider/provider.dart';

class Language {
  static Future<void> changeLanguage(BuildContext context, String language,
      [bool pop = false]) async {
    await changeLocale(context, language);
    updateLanguagePref(language);
    if (pop) {
      Navigator.pop(context, language);
    }

    context.read<ActiveHours>().calibrateDayNames();
  }
}
