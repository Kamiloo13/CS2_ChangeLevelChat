[![GitHub Downloads](https://img.shields.io/github/downloads/Kamiloo13/CS2_ChangeLevelChat/total.svg?style=flat-square&label=Downloads)](https://github.com/Kamiloo13/CS2_ChangeLevelChat/releases/latest)

# CS2 Change Level by Chat
> Written in C# for **Counter-Strike 2** using [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp)

Allows users to change the map and game mode by in-game chat. 
- **Game modes** are just simple config files stored in `csgo/cfg/` directory, those allow for a fast game_type change (ex. from aim_based config to smoke_training one).
- This plugin isn't planned to be used on a public server, it's more for private servers where you want to play with friends that you trust. 
- Can be toggled off with a command or in config by default.
- This plugin **IS NOT** a voting system and **IT IS** an instant map change. If you want a voting system instead, you can use [cs2-rockthevote](https://github.com/abnerfs/cs2-rockthevote) plugin.

### Setup
* Make sure your server has [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp) and [Metamod](https://www.sourcemm.net/downloads.php/?branch=master) properly installed. You can find the instalation instructions [here](https://docs.cssharp.dev/docs/guides/getting-started.html).
* Download the zip file from the latest [release](https://github.com/Kamiloo13/CS2_ChangeLevelChat/releases), and extract the contents into your `csgo/addons/counterstrikesharp/plugins` directory.
* Configuration file is located in `csgo/addons/counterstrikesharp/configs/plugins/ChangeLevelChat/ChangeLevelChat.json`. You can change the maps and gamemodes avaivable there. *It will be created and read on server boot or hot reload*.


### Commands
| Command         | Arguments                         | Description                                                          | Permissions |
|-----------------|-----------------------------------|----------------------------------------------------------------------|-------------|
| !changelevel    | \<mapname\> \<gamemode?\>         | Change the map and if specified game mode                            | everyone    |
| !changemode     | \<gamemode>                       | Change the game mode                                                 | everyone    |
| !maps           |                                   | List all maps (configured in config)                                 | everyone    |
| !modes          |                                   | List all gamemodes (configured in config)                            | everyone    |
| !changetoggle   |                                   | Toggle if users can use this plugin                                  | @css/admin  |

### Roadmap
- [ ] Add permissions for commands
