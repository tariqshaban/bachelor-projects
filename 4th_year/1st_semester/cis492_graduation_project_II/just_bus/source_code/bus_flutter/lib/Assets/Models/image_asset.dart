class ImageAsset {
  late String directory;
  late String name;
  late bool is360;

  ImageAsset(
      {required this.directory, required this.name, required this.is360});

  String getFullPath() {
    return directory + name;
  }

  factory ImageAsset.fromJson(Map<String, dynamic> parsedJson) {
    return ImageAsset(
      directory: parsedJson['directory'],
      name: parsedJson['name'],
      is360: parsedJson['is360'],
    );
  }

  Map<String, dynamic> toJson() => {
        'directory': directory,
        'name': name,
        'is360': is360,
      };
}
