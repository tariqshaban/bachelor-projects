import 'package:bloc/bloc.dart';
import 'package:bus/Assets/Helpers/prefs.dart';
import 'package:bus/Assets/ThemesHandler/bloc/theme_event.dart';
import 'package:bus/Assets/ThemesHandler/bloc/theme_state.dart';

class ThemeBloc extends Bloc<ToggleThemeEvent, ThemeState> {
  ThemeBloc()
      : super(ThemeState.init(prefs.getBool('isDarkTheme') != null
            ? prefs.getBool('isDarkTheme') is bool
            : false)) {
    on<ToggleThemeEvent>((event, emit) {
      if (prefs.getBool('isDarkTheme') != null &&
          state.isDarkMode != prefs.getBool('isDarkTheme')) {
        emit(ThemeState(isDarkMode: !state.isDarkMode));
      } else {
        emit(ThemeState(isDarkMode: state.isDarkMode));
      }
    });
  }
}
