# Changelog

## unreleased (0.7.0)

From this release, the library has been completely resshufled to a simpler library focusing on full compliance with STAC specifications

### Added

- Json Schema validation
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