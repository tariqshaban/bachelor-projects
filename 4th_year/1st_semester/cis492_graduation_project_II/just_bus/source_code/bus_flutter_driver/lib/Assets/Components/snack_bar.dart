import 'package:flutter/material.dart';

class SnackBars {
  static showTextSnackBar(BuildContext context, String message) {
    SnackBar snackBar = SnackBar(content: Text(message));

    ScaffoldMessenger.of(context).showSnackBar(snackBar);
  }
}
