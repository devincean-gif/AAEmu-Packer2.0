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
executables). Edit `readers.json` to add your own custom formats.

## AAPakCLI quick reference

```
AAPakCLI <pak>                  open read-only (for exporting)
AAPakCLI -o  <pak>              open read/write
AAPakCLI +c  <pak>              create a STANDARD (WIBO) pak  ← server format
AAPakCLI +ca <pak>              create an AAFree (ZERO) pak   ← client format
+d <sourcedir> <pakdir>         add a folder tree into the pak
-u <pakdir>     <destdir>       extract a folder to disk
-l <sourcefile> <destfile>      extract a single file
-csv <file>                     export the file list as CSV
-x  /  +x                       save & close  ( +x also exits )
```

Run `AAPakCLI -?` for the complete argument list. Arguments are processed in
order, so you can chain operations.

## Creating a pak with AAPakCLI

Arguments run left-to-right: create the pak, add files, then save & exit.

### Server-side pak (standard / WIBO format)

AAEmu servers read a standard **WIBO** pak. Create one from a folder of server
files with **`+c`** (here the output is named so it can be dropped straight into
the server's `ClientData`):

```
AAPakCLI +c aafree_5070_server_game_pak +d C:\serverdata\game game +x
```

Then point the server's `Configurations/ClientData.json` `Sources` entry at that
file (the filename itself is arbitrary — the path in `ClientData.json` is what
matters).

### Client-side pak (AAFree 5.0 "ZERO" format)

Use **`+ca`** instead to write the AAFree client format:

```
AAPakCLI +ca game_pak +d C:\clientdata\game game +x
```

`+d <sourcedir> <pakdir>` adds a whole directory tree; chain extra `+d`/`+f`
arguments to add more, then `+x` to save and exit.

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
