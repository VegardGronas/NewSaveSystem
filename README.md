# Version - 0.1.0

# WorldKeeper
A saving system for Unity.

# Future updates

## Versioning / Compatibility Y
### A version number in the save file so future changes to components or prefabs don’t break old saves.
#### Added version and timestamp.

## Partial Save / Incremental Save
### Optionally save only changed objects instead of the whole scene.

## Undo / Backup system
### Backup last N saves automatically, so a bad save doesn’t break testing.

## Error Handling / Logging
### Mostly have warnings for missing prefabs, but maybe also log failed loads, corrupted JSON, or duplicated IDs.

## Optional Encryption / Compression
### Useful to prevent tampering or reduce disk usage.

## Event hooks Y 
### Already have OnContentLoaded, but maybe also OnBeforeSave, OnAfterSave, OnBeforeLoad, OnAfterLoad for more flexibility

## Runtime-only objects flag
### For objects that shouldn’t exist in the scene by default but need saving when spawned. Kind of already handle this with prefab-path loading, but a clearer distinction could help.

## Save/Load timestamps
### For profiling or debug purposes, showing when a save was made.