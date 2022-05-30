import 'package:bus_driver/Assets/Enums/test_status.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter_translate/flutter_translate.dart';

class Reachability with ChangeNotifier {
  TestStatus _status = TestStatus.pending;
  String _entryTime = translate('reachability_dialog.pending');
  String _dbTime = translate('reachability_dialog.pending');
  String _returnTime = translate('reachability_dialog.pending');
  String _rtt = translate('reachability_dialog.pending');

  TestStatus get status => _status;

  String get entryTime => _entryTime;

  String get dbTime => _dbTime;

  String get returnTime => _returnTime;

  String get rtt => _rtt;

  setResults(String entryTime, String dbTime, String returnTime, String rtt) {
    _entryTime = entryTime;
    _dbTime = dbTime;
    _returnTime = returnTime;
    _rtt = rtt;

    if (entryTime == '-' || dbTime == '-' || returnTime == '-' || rtt == '-') {
      _status = TestStatus.failed;
    } else {
      _status = TestStatus.success;
    }

    notifyListeners();
  }

  resetResults() {
    _entryTime = translate('reachability_dialog.pending');
    _dbTime = translate('reachability_dialog.pending');
    _returnTime = translate('reachability_dialog.pending');
    _rtt = translate('reachability_dialog.pending');

    _status = TestStatus.pending;
    notifyListeners();
  }
}
