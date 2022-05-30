class ThemeState {
  final bool isDarkMode;

  ThemeState({required this.isDarkMode});

  factory ThemeState.init(bool isDarkMode) =>
      ThemeState(isDarkMode: isDarkMode);
}
