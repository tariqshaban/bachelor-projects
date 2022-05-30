import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

ThemeData buildThemeOne() {
  SystemChrome.setSystemUIOverlayStyle(
    const SystemUiOverlayStyle(
      systemNavigationBarColor: Color(0xfffafafa),
      systemNavigationBarIconBrightness: Brightness.dark,
    ),
  );

  return ThemeData.light().copyWith(
    primaryColor: const Color(0xFF0E8B8B),
    colorScheme: ThemeData.light().colorScheme.copyWith(
      primary: const Color(0xFF0E8B8B),
      secondary: const Color(0xFF99D0D0),
    ),
    dividerColor: Colors.black,
    appBarTheme: const AppBarTheme(
      iconTheme: IconThemeData(
        color: Color(0xFF0E8B8B),
      ),
      titleTextStyle: TextStyle(
        fontWeight: FontWeight.bold,
        fontSize: 20,
        color: Colors.black,
      ),
    ),
  );
}

ThemeData buildThemeTwo() {
  SystemChrome.setSystemUIOverlayStyle(
    const SystemUiOverlayStyle(
      systemNavigationBarColor: Color(0xff303030),
      systemNavigationBarIconBrightness: Brightness.light,
    ),
  );

  return ThemeData.dark().copyWith(
    primaryColor: const Color(0xFF0E8B8B),
    colorScheme: ThemeData.dark().colorScheme.copyWith(
      primary: const Color(0xFF0E8B8B),
      secondary: const Color(0xFF99D0D0),
    ),
    dividerColor: Colors.white,
    appBarTheme: const AppBarTheme(
      iconTheme: IconThemeData(
        color: Color(0xFF0E8B8B),
      ),
      titleTextStyle: TextStyle(
        fontWeight: FontWeight.bold,
        fontSize: 20,
        color: Colors.white,
      ),
    ),
  );
}
