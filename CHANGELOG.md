# Changelog

## [Unreleased]

### Added

### Fixed

### Changed

### Removed

## [v0.8.1]

### Added

- Root fields accessor for Stac Item
- Extensible raster object

### Fixed

- gsd as double
- null value handling (e.g. proj:epsg)
- ObservationDirection as enum

### Changed

## [v0.8.0]

Stac 1.0.0-rc.4 compatible release

### Added

- Raster extension
- Version extension
- Virtual Assets extension
- Processing extension completed
- File extension
- More descriptive exceptions for deserialization

### Fixed

- Internal contructors for extension supported by the extension manager

### Changed

- Current version set to 1.0.0-rc.4

## [v0.7.0]

From this release, the library has been completely reshuffled to a simpler library focusing on full compliance with STAC specifications.
At the moment of this release, the STAC specification is 1.0.0-rc.4 and minor changes in the spec should not break this implementation.

### Added

- Json Schema validation and validator class
- All missing object implementation to target STAC specification 1.0.0
- Collection creation helper with automatic summaries

### Changed

- Extensions helpers approach. Extensions classes are created when calling the extented methods of the STAC object and are not assigned to the STAC object anymore.

### Removed

- Data access middleware and methods. All remote access to the file are left to the developer discretion.

## [v0.6.2]

### Added

- Helper to set projection WKT2 by EPSG code
- Link creation from a STAC Object

### Fixed

- Content-type fixed
- Avoid null items in summaries

### Changed

- Cloned assets

[Unreleased]: <https://github.com/Terradue/DotnetStac/compare/0.8.1...HEAD>
[v0.8.0]: <https://github.com/Terradue/DotnetStac/tree/0.8.1>
[v0.8.0]: <https://github.com/Terradue/DotnetStac/tree/0.8.0>
[v0.7.0]: <https://github.com/Terradue/DotnetStac/tree/0.7.0>
[v0.6.2]: <https://github.com/Terradue/DotnetStac/tree/0.6.2>