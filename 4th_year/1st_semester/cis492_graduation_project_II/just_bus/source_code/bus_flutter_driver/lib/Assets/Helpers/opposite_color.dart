import 'package:flutter/material.dart';

class OppositeColor {
  static Color oppositeTextColor(BuildContext context) {
    Color color = Theme.of(context).textTheme.headline6!.color!;

    if (color == Colors.white) {
      return const Color(0xFF303030);
    }

    return Colors.white;
  }
}
