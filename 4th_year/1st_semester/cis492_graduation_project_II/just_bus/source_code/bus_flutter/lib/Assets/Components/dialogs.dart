import 'dart:convert';

import 'package:bus/Assets/Enums/test_status.dart';
import 'package:bus/Assets/Models/Providers/Dialogs/reachability.dart';
import 'package:bus/Assets/Models/server_status.dart';
import 'package:flutter/material.dart';
import 'package:flutter_translate/flutter_translate.dart';
import 'package:http/http.dart' as http;
import 'package:ntp/ntp.dart';
import 'package:provider/provider.dart';

class Dialogs {
  static getProgressDialog(BuildContext context, String message) {
    AlertDialog alert = AlertDialog(
      content: Row(
        children: [
          CircularProgressIndicator(
            color: Theme.of(context).primaryColor,
          ),
          const SizedBox(width: 20),
          FittedBox(fit: BoxFit.fitWidth, child: Text(message)),
        ],
      ),
    );

    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (BuildContext context) {
        return WillPopScope(
            onWillPop: () {
              return Future.value(false);
            },
            child: alert);
      },
    );

    return alert;
  }

  static getReachabilityTestDialog(BuildContext context) {
    context.read<Reachability>().resetResults();

    AlertDialog alert = AlertDialog(
      content: Consumer<Reachability>(
        builder: (context, reachability, child) {
          return Row(
            children: [
              Expanded(
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    SizedBox(
                      height: 25,
                      child: Align(
                        alignment: AlignmentDirectional.centerStart,
                        child: FittedBox(
                          fit: BoxFit.fitWidth,
                          child: Text(
                            _getTestStatusText(context, reachability),
                            style: TextStyle(
                                color: Theme.of(context).primaryColor),
                          ),
                        ),
                      ),
                    ),
                    const SizedBox(height: 30),
                    Text(translate('reachability_dialog.entry')),
                    const SizedBox(height: 10),
                    Text(translate('reachability_dialog.db')),
                    const SizedBox(height: 10),
                    Text(translate('reachability_dialog.return')),
                    const SizedBox(height: 30),
                    Text(
                      translate('reachability_dialog.rtt'),
                      style: TextStyle(color: Theme.of(context).primaryColor),
                    ),
                  ],
                ),
              ),
              Column(
                mainAxisSize: MainAxisSize.min,
                crossAxisAlignment: CrossAxisAlignment.end,
                children: [
                  _getTestStatusWidget(context, reachability),
                  const SizedBox(height: 30),
                  Text(reachability.entryTime),
                  const SizedBox(height: 10),
                  Text(reachability.dbTime),
                  const SizedBox(height: 10),
                  Text(reachability.returnTime),
                  const SizedBox(height: 30),
                  Text(
                    reachability.rtt,
                    style: TextStyle(color: Theme.of(context).primaryColor),
                  ),
                ],
              ),
            ],
          );
        },
      ),
    );

    showDialog(
      context: context,
      builder: (BuildContext context) {
        return alert;
      },
    );

    _initiatePingTest(context);

    return alert;
  }

  static _initiatePingTest(BuildContext context) async {
    try {
      DateTime beforeRequest =
          (await NTP.now().timeout(Duration(seconds: ServerStatus.timeout)))
              .toUtc();
      DateTime apiEntryTime;
      DateTime queryRetrievalTime;
      DateTime afterRequest;

      if (beforeRequest.year == 0) {
        throw ('Utc Time Api Call Failed');
      }

      final response = await http
          .get(Uri.parse('${ServerStatus.serverUrl}/api/Ping'))
          .timeout(Duration(seconds: ServerStatus.timeout));
      if (response.statusCode == 200) {
        afterRequest =
            (await NTP.now().timeout(Duration(seconds: ServerStatus.timeout)))
                .toUtc();

        dynamic responseJson = json.decode(response.body);
        apiEntryTime = DateTime.parse(responseJson['apiEntryTime']);
        queryRetrievalTime = DateTime.parse(responseJson['queryRetrievalTime']);

        context.read<Reachability>().setResults(
              _msToString(
                  apiEntryTime.difference(beforeRequest).inMilliseconds),
              _msToString(
                  queryRetrievalTime.difference(apiEntryTime).inMilliseconds),
              _msToString(
                  afterRequest.difference(queryRetrievalTime).inMilliseconds),
              _msToString(
                  afterRequest.difference(beforeRequest).inMilliseconds),
            );
      } else if (response.statusCode == 404) {
        afterRequest =
            (await NTP.now().timeout(Duration(seconds: ServerStatus.timeout)))
                .toUtc();

        dynamic responseJson = json.decode(response.body);
        apiEntryTime = DateTime.parse(responseJson['apiEntryTime']);
        queryRetrievalTime = DateTime.parse(responseJson['queryRetrievalTime']);

        context.read<Reachability>().setResults(
              _msToString(
                  apiEntryTime.difference(beforeRequest).inMilliseconds),
              '-',
              _msToString(
                  afterRequest.difference(queryRetrievalTime).inMilliseconds),
              _msToString(
                  afterRequest.difference(beforeRequest).inMilliseconds),
            );
      } else {
        context.read<Reachability>().setResults(
              '-',
              '-',
              '-',
              '-',
            );
      }
    } catch (_) {
      context.read<Reachability>().setResults(
            '-',
            '-',
            '-',
            '-',
          );
    }
  }

  static String _msToString(int millisecond) {
    if (millisecond < 0) millisecond = 1;

    if (millisecond >= 1000) {
      return '${(millisecond / 1000).toStringAsFixed(1)} ${translate('reachability_dialog.second')}';
    }

    return '$millisecond ${translate('reachability_dialog.millisecond')}';
  }

  static Widget _getTestStatusWidget(
      BuildContext context, Reachability reachability) {
    if (reachability.status == TestStatus.pending) {
      return SizedBox(
        width: 25,
        height: 25,
        child: CircularProgressIndicator(
          color: Theme.of(context).primaryColor,
          strokeWidth: 3,
        ),
      );
    } else if (reachability.status == TestStatus.success) {
      return Icon(
        Icons.check,
        color: Theme.of(context).primaryColor,
        size: 25,
      );
    } else {
      return Icon(
        Icons.clear,
        color: Theme.of(context).primaryColor,
        size: 25,
      );
    }
  }

  static String _getTestStatusText(
      BuildContext context, Reachability reachability) {
    if (reachability.status == TestStatus.pending) {
      return translate('reachability_dialog.checking');
    } else if (reachability.status == TestStatus.success) {
      return translate('reachability_dialog.finished');
    } else {
      return translate('reachability_dialog.failed');
    }
  }
}
