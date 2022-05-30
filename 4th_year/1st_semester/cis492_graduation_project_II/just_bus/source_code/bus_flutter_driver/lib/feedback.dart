import 'dart:convert';
import 'dart:io';

import 'package:device_info_plus/device_info_plus.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_translate/flutter_translate.dart';
import 'package:provider/provider.dart';

import 'Assets/Components/dialogs.dart';
import 'Assets/Components/snack_bar.dart';
import 'Assets/Helpers/role_based_http_handler.dart';
import 'Assets/Models/Providers/Feedback/checkbox_value.dart';
import 'Assets/Models/Providers/Feedback/input_text_error_message.dart';
import 'Assets/Models/feedback_model.dart';
import 'Assets/Models/inspection.dart';
import 'Assets/Models/server_status.dart';

class Feedback extends StatefulWidget {
  const Feedback({Key? key}) : super(key: key);

  @override
  _FeedbackState createState() => _FeedbackState();
}

class _FeedbackState extends State<Feedback> {
  TextEditingController phoneNumberText = TextEditingController();
  TextEditingController feedbackText = TextEditingController();

  @override
  void initState() {
    Future(() {
      Inspection.inspect(context);
    });

    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    SystemChrome.setSystemUIOverlayStyle(
        SystemUiOverlayStyle(statusBarColor: Theme.of(context).primaryColor));
    return GestureDetector(
      onTap: () {
        FocusScope.of(context).requestFocus(FocusNode());
      },
      child: Scaffold(
        appBar: AppBar(
          systemOverlayStyle: SystemUiOverlayStyle(
              statusBarColor: Theme.of(context).primaryColor),
          backgroundColor: Colors.transparent,
          elevation: 0,
        ),
        body: SafeArea(
          child: Center(
            child: SingleChildScrollView(
              child: Padding(
                padding: const EdgeInsets.all(30),
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  crossAxisAlignment: CrossAxisAlignment.center,
                  children: <Widget>[
                    Consumer<InputTextErrorMessage>(
                      builder: (context, inputTextErrorMessage, child) {
                        return TextField(
                          controller: phoneNumberText,
                          maxLength: 10,
                          style:
                              TextStyle(color: Theme.of(context).primaryColor),
                          cursorColor: Theme.of(context).primaryColor,
                          keyboardType: TextInputType.phone,
                          inputFormatters: <TextInputFormatter>[
                            FilteringTextInputFormatter.allow(RegExp(r'[0-9]')),
                            FilteringTextInputFormatter.allow(
                                RegExp(r'(07[789]\d{0,})|(07)|(^0$)')),
                          ],
                          decoration: InputDecoration(
                            contentPadding: const EdgeInsets.only(top: 0.0),
                            errorText: (inputTextErrorMessage.errorMessage == 1)
                                ? translate('feedback.invalid_phone')
                                : null,
                            isDense: true,
                            focusedBorder: UnderlineInputBorder(
                              borderSide: BorderSide(
                                  color: Theme.of(context).primaryColor),
                            ),
                            labelText: translate('feedback.phone'),
                            labelStyle: TextStyle(
                                color: Theme.of(context).primaryColor,
                                fontSize: 12),
                            hintText: "07xxxxxxxx",
                            hintStyle: TextStyle(
                                color: Theme.of(context)
                                    .primaryColor
                                    .withOpacity(0.6),
                                fontSize: 12),
                          ),
                        );
                      },
                    ),
                    const Padding(
                      padding: EdgeInsets.only(top: 15),
                    ),
                    Consumer<InputTextErrorMessage>(
                      builder: (context, inputTextErrorMessage, child) {
                        return TextField(
                          minLines: 1,
                          maxLines: 50,
                          controller: feedbackText,
                          maxLength: 400,
                          style:
                              TextStyle(color: Theme.of(context).primaryColor),
                          cursorColor: Theme.of(context).primaryColor,
                          decoration: InputDecoration(
                            contentPadding: const EdgeInsets.only(top: 0.0),
                            errorText: (inputTextErrorMessage.errorMessage == 2)
                                ? translate('feedback.message_cannot_be_empty')
                                : ((inputTextErrorMessage.errorMessage == 3)
                                    ? translate('feedback.bit_longer')
                                    : null),
                            isDense: true,
                            focusedBorder: UnderlineInputBorder(
                              borderSide: BorderSide(
                                  color: Theme.of(context).primaryColor),
                            ),
                            labelText: translate('feedback.feedback'),
                            labelStyle: TextStyle(
                                color: Theme.of(context).primaryColor,
                                fontSize: 12),
                          ),
                        );
                      },
                    ),
                    const Padding(
                      padding: EdgeInsets.only(top: 15),
                    ),
                    Container(
                      margin: const EdgeInsets.fromLTRB(15, 0, 15, 0),
                      child: Consumer<CheckboxValue>(
                        builder: (context, checkboxValue, child) {
                          return CheckboxListTile(
                            value: checkboxValue.isEnabled,
                            controlAffinity: ListTileControlAffinity.leading,
                            title: Text(
                              translate('feedback.include_os'),
                              style: const TextStyle(fontSize: 12),
                            ),
                            activeColor: Theme.of(context).primaryColor,
                            onChanged: (value) {
                              checkboxValue.isEnabled =
                                  !checkboxValue.isEnabled;
                            },
                          );
                        },
                      ),
                    ),
                    const Padding(
                      padding: EdgeInsets.only(top: 15),
                    ),
                    ElevatedButton(
                      style: ButtonStyle(
                        minimumSize:
                            MaterialStateProperty.all(const Size(200, 0)),
                        backgroundColor: MaterialStateProperty.all(
                            Theme.of(context).primaryColor),
                        padding: MaterialStateProperty.all<EdgeInsets>(
                            const EdgeInsets.all(10)),
                        shape:
                            MaterialStateProperty.all<RoundedRectangleBorder>(
                          RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(18.0),
                          ),
                        ),
                      ),
                      child: Text(translate('feedback.submit').toUpperCase(),
                          style: const TextStyle(fontSize: 14)),
                      onPressed: _sendFeedback,
                    ),
                  ],
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }

  Future<void> _sendFeedback() async {
    if (phoneNumberText.text.length != 10 && phoneNumberText.text.isNotEmpty) {
      context.read<InputTextErrorMessage>().errorMessage = 1;
    } else if (feedbackText.text.isEmpty) {
      context.read<InputTextErrorMessage>().errorMessage = 2;
    } else if (feedbackText.text.length < 15) {
      context.read<InputTextErrorMessage>().errorMessage = 3;
    } else {
      context.read<InputTextErrorMessage>().errorMessage = -1;

      FeedbackModel feed = FeedbackModel();
      feed.phoneNumber =
          phoneNumberText.text.isEmpty ? null : phoneNumberText.text;
      feed.message = feedbackText.text;
      if (context.read<CheckboxValue>().isEnabled) {
        feed.model = (Platform.isAndroid)
            ? (await DeviceInfoPlugin().androidInfo).model
            : (await DeviceInfoPlugin().iosInfo).model;
        feed.osVersion = (Platform.isAndroid)
            ? 'Android ${(await DeviceInfoPlugin().androidInfo).version.release!} Sdk= ${(await DeviceInfoPlugin().androidInfo).version.sdkInt.toString()}'
            : 'iOS ${(await DeviceInfoPlugin().iosInfo).systemVersion!}';
      }

      Dialogs.getProgressDialog(
          context, translate('feedback.sending_feedback'));

      try {
        final response =
            await RoleBasedHttpHandler.handleRoleBasedHttpPostRequest(
          Uri.parse('${ServerStatus.serverUrl}/api/Feedbacks'),
          true,
          jsonEncode(feed.toJson()),
        ).timeout(Duration(seconds: ServerStatus.timeout));

        if (response.statusCode == 201) {
          SnackBars.showTextSnackBar(
              context, translate('feedback.thank_you_feedback'));
          Navigator.pop(context);
          Navigator.pop(context);
        } else if (response.statusCode == 429) {
          SnackBars.showTextSnackBar(
              context, translate('api_response.too_many_requests'));
        } else {
          SnackBars.showTextSnackBar(
              context, translate('api_response.server_error'));
        }
      } catch (_) {
        SnackBars.showTextSnackBar(
            context, translate('api_response.timed_out'));
      }

      Navigator.pop(context);
    }
  }
}
