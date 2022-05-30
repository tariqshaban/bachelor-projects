import 'package:flutter/cupertino.dart';

class PaintRectangle extends CustomPainter {
  final Color primaryColor;
  final Color? secondaryColor;

  PaintRectangle({
    required this.primaryColor,
    required this.secondaryColor,
  });

  @override
  void paint(Canvas canvas, Size size) {
    final y = size.height;
    final x = size.width;

    Paint paint = Paint()..color = primaryColor;
    Path path = Path();

    if (secondaryColor != null) {
      path
        ..lineTo(x, y)
        ..lineTo(0, y);
    } else {
      path
        ..lineTo(x, 0)
        ..lineTo(x, y)
        ..lineTo(0, y);
    }

    canvas.drawPath(path, paint);

    if (secondaryColor != null) {
      paint = Paint()..color = secondaryColor!;
      path = Path();

      path
        ..lineTo(x, y)
        ..lineTo(x, 0);

      canvas.drawPath(path, paint);
    }
  }

  @override
  bool shouldRepaint(CustomPainter oldDelegate) {
    return true;
  }
}
