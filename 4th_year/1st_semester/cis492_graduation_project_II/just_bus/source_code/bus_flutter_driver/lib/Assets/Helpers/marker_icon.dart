import 'dart:typed_data';
import 'dart:ui' as ui;

import 'package:flutter/cupertino.dart';
import 'package:flutter/services.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';

class MarkerIcon {
  static Map<String, BitmapDescriptor> icons = {};

  static late double height;
  static late double width;

  static void assignBitmapDescriptors(BuildContext context) async {
    double height = MediaQuery.of(context).size.height *
        MediaQuery.of(context).devicePixelRatio;
    double width = MediaQuery.of(context).size.width *
        MediaQuery.of(context).devicePixelRatio;

    MarkerIcon.height = height;
    MarkerIcon.width = width;

    icons['User'] = await _bitmapDescriptorFromSvgAsset(
        context, 'assets/images/you.svg', 35);
    icons['User_driver'] = await _bitmapDescriptorFromSvgAsset(
        context, 'assets/images/you_driver.svg', 25);
    icons['Stop_l'] = await _bitmapDescriptorFromSvgAsset(
        context, 'assets/images/stop.svg', 55);
    icons['Stop_s'] = await _bitmapDescriptorFromSvgAsset(
        context, 'assets/images/stop.svg', 40);
    icons['StopReturn_l'] = await _bitmapDescriptorFromSvgAsset(
        context, 'assets/images/stop_return.svg', 55);
    icons['StopReturn_s'] = await _bitmapDescriptorFromSvgAsset(
        context, 'assets/images/stop_return.svg', 40);
    icons['Bus'] = await _bitmapDescriptorFromSvgAsset(
        context, 'assets/images/bus.svg', 25);
    icons['Bus_deviate'] = await _bitmapDescriptorFromSvgAsset(
        context, 'assets/images/bus_deviate.svg', 25);
    icons['Bus_return'] = await _bitmapDescriptorFromSvgAsset(
        context, 'assets/images/bus_return.svg', 25);
  }

  static Future<BitmapDescriptor> _bitmapDescriptorFromSvgAsset(
      BuildContext context, String assetName, int size) async {
    String svgString =
        await DefaultAssetBundle.of(context).loadString(assetName);
    DrawableRoot svgDrawableRoot = await svg.fromSvgString(svgString, 'dummy');

    MediaQueryData queryData = MediaQuery.of(context);
    double devicePixelRatio = queryData.devicePixelRatio;
    double width = size * devicePixelRatio;
    double height = size * devicePixelRatio;

    ui.Picture picture = svgDrawableRoot.toPicture(size: Size(width, height));

    ui.Image image = await picture.toImage(width.round(), height.round());
    ByteData? bytes = await image.toByteData(format: ui.ImageByteFormat.png);
    return BitmapDescriptor.fromBytes(bytes!.buffer.asUint8List());
  }
}
