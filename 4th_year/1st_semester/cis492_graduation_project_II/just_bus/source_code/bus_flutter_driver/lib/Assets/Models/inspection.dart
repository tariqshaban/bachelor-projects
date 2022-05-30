import 'package:flutter/cupertino.dart';

class Inspection {
  static bool isInspectionComplete = false;

  static inspect(BuildContext context) {
    if (!isInspectionComplete) {
      Navigator.of(context)
          .pushNamedAndRemoveUntil('/', (Route<dynamic> route) => false);
    }
  }
}
