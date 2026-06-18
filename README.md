# AAEmu-Packer 2.0

Tools to **read, edit, and build ArcheAge `game_pak` files** — including the
**AAFree 5.0 "ZERO"** client format, alongside the standard XLGames/WIBO format
used by retail clients and AAEmu servers.

Based on [ZeromusXYZ/AAEmu-Packer](https://github.com/ZeromusXYZ/AAEmu-Packer),
extended with AAFree 5.0 support.

## Tools

| Tool | Description |
|------|-------------|
| **AAPakEditor** | GUI editor — open a `game_pak`, browse/extract/add/replace/delete files, save. AAFree paks auto-detect; pick the format from the reader menu when creating a new pak. |
| **AAPakCLI** | Scriptable command-line packer / extractor. |
| **AAMod** | Create and apply `.aamod` patch files. |

## AAFree 5.0 support

The AAFree client repacks `game_pak` in a custom **"ZERO"** variant (different
header magic and an 8-byte name-field prefix) instead of the standard "WIBO"
layout. This build adds a format **reader** for that variant, so every tool can
read, edit, and create AAFree paks as well as standard ones.

The format readers are defined in `readers.json` (placed next to the
executables) and the AAFree reader is also built in, so the tools work even
without that file. Edit `readers.json` to add your own custom formats.

## AAPakCLI quick reference

```
AAPakCLI <pak>                  open read-only (for exporting)
AAPakCLI -o  <pak>              open read/write
AAPakCLI +c  <pak>              create a STANDARD (WIBO) pak
AAPakCLI +ca <pak>              create an AAFree (ZERO) pak
+d <sourcedir> <pakdir>         add a folder tree into the pak
-u <pakdir>     <destdir>       extract a folder to disk
-l <sourcefile> <destfile>      extract a single file
-csv <file>                     export the file list as CSV
-x  /  +x                       save & close  ( +x also exits )
```

Run `AAPakCLI -?` for the complete argument list. Arguments are processed in
order, so you can chain operations, e.g.:

```
AAPakCLI +ca my_game_pak +d C:\modfiles\game game +x
```

## Building

Requires the **.NET 8 SDK**.

```
dotnet build AAPakEditor.sln -c Release
```

To produce a portable, self-contained single-file build of a tool:

```
dotnet publish AAPakEditor/AAPakEditor.csproj -c Release -r win-x64 ^
    --self-contained true -p:PublishSingleFile=true
```

## Credits

- Original AAEmu-Packer and the AAPak library: **ZeromusXYZ**
- AAFree 5.0 (ZERO) format support: **Devincean**

## License

See [LICENSE](LICENSE).
