import 'dart:core';

class FeedbackModel {
  String? model;
  String? osVersion;
  String? message;
  String? phoneNumber;

  Map<String, dynamic> toJson() => {
        'message': message,
        'model': model,
        'os': osVersion,
        'phone': phoneNumber,
      };
}
