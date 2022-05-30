import 'package:flutter/material.dart';

class CustomDropdownMenuItem<T> extends DropdownMenuItem<T> {
  final double height;

  const CustomDropdownMenuItem(
      {Key? key,
      onTap,
      value,
      enabled = true,
      this.height = 36,
      AlignmentGeometry alignment = AlignmentDirectional.centerStart,
      Widget child = const Text('')})
      : super(
            key: key,
            onTap: onTap,
            value: value,
            enabled: enabled,
            alignment: alignment,
            child: child);

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsetsDirectional.only(start: 10),
      child: SizedBox(
        height: height,
        child: Container(
          alignment: alignment,
          child: child,
        ),
      ),
    );
  }
}
