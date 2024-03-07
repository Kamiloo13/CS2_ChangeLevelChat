using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Timers;

namespace ChangeLevelChat
{
    public class ChangeLevel : BasePlugin, IPluginConfig<Config>
    {
        private const string Version = "1.0.0";

        #region Plugin Info
        public override string ModuleName => "Change Level Chat";
        public override string ModuleVersion => Version;
        public override string ModuleAuthor => "Kamiloo13";
        public override string ModuleDescription => "Lets you change the level by typing a command in chat.";
        #endregion

        public required Config Config { get; set; } = new Config();
        public string? CurrentGameMode = "aim_maps";

        public override void Load(bool hotReload)
        {
            RegisterListener<Listeners.OnMapStart>(OnMapStart);

            if (hotReload)
            {
                OnMapStart(null);
            }
        }

        public void OnConfigParsed(Config config)
        {
            PHelpers.SetLogLevel(config.LogLevel);
            if (config.Version < Config.Version)
            {
                PHelpers.Error($"Configuration version mismatch (Expected: {Config.Version} | Current: {config.Version})");
            }

            Config = config;

            PHelpers.Debug("Finished Loading Config...");
        }

        private void OnMapStart(string? mapName)
        {
            PHelpers.Debug("Executing OnMapStart");

            AddTimer(5.0f, () =>
            {
                if (CurrentGameMode != null)
                {
                    PHelpers.Debug($"Executing {CurrentGameMode}");
                    Server.ExecuteCommand($"exec {CurrentGameMode}");
                }
            }, TimerFlags.STOP_ON_MAPCHANGE);
        }

        private bool IsEnabled()
        {
            if (!Config.IsEnabled)
            {
                PHelpers.Debug("Plugin is disabled...");
            }

            return Config.IsEnabled;
        }

        #region Commands

        [ConsoleCommand("css_changelevel", "Change the level.")]
        [CommandHelper(whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
        public void OnChangeLevelCommand(CCSPlayerController? player, CommandInfo commandInfo)
        {
            PHelpers.Debug("changelevel command handle");
            if (!IsEnabled()) return;

            if (commandInfo.ArgCount < 2)
            {
                PHelpers.ReplyToCmd(commandInfo, "Usage: css_changelevel <mapname> <gamemode?> (e.g. css_changelevel de_dust2 training)");
                return;
            }
            string map = commandInfo.GetArg(1);
            string? gamemode = commandInfo.GetArg(2);

            if (gamemode != null)
            {
                if (Config.GameModes.Contains(gamemode))
                {
                    PHelpers.Log($"Changing gamemode to {gamemode}");
                    CurrentGameMode = gamemode;
                    Server.ExecuteCommand($"exec {gamemode}");
                } 
                else if (gamemode == "none")
                {
                    Server.ExecuteCommand("exec gamemode_competitive");
                    CurrentGameMode = null;
                }
            }

            if (Config.Maps.Contains(map))
            {
                PHelpers.Log($"Changing level to {map}");
                Server.ExecuteCommand($"changelevel {map}");
            } 
            else if (Config.WorkshopMaps.Contains(map))
            {
                PHelpers.Log($"Changing level to {map}");
                Server.ExecuteCommand($"ds_workshop_changelevel {map}");
            }
            else
            {
                PHelpers.Log($"Couldn't find that map on the list... {map}");
                PHelpers.ReplyToCmd(commandInfo, "Couldn't find that map on the list... Are you sure it's listed in !maps?");
            }
        }

        [ConsoleCommand("css_changemode", "Change the game mode.")]
        [CommandHelper(whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
        public void OnChangeGameCommand(CCSPlayerController? player, CommandInfo commandInfo)
        {
            PHelpers.Debug("changegame command handle");
            if (!IsEnabled()) return;

            if (commandInfo.ArgCount < 2)
            {
                PHelpers.ReplyToCmd(commandInfo, "Usage: css_changegame <gamemode> (e.g. css_changegame smoke_training)");
                return;
            }

            string gamemode = commandInfo.GetArg(1);

            if (gamemode == "none")
            {
                PHelpers.PrintToAll("Changing gamemode to default...");
                Server.ExecuteCommand("exec gamemode_competitive");
                CurrentGameMode = null;
                return;
            }

            if (Config.GameModes.Contains(gamemode))
            {
                PHelpers.PrintToAll($"Changing gamemode to {gamemode}");
                Server.ExecuteCommand($"exec {gamemode}");
                CurrentGameMode = gamemode;
                return;
            }

            PHelpers.ReplyToCmd(commandInfo, "Couldn't find that map on the list... Are you sure it's listed in !gamemodes?");
        }

        [ConsoleCommand("css_maps", "List all maps.")]
        [CommandHelper(whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
        public void OnListMapsCommand(CCSPlayerController? player, CommandInfo commandInfo)
        {
            PHelpers.Debug("Listing maps...");
            if (!IsEnabled()) return;

            if (Config.Maps.Length > 0)
            {
                PHelpers.PrintToAll("---- CS2 maps ---- ");
                for (int i = 0; i < Config.Maps.Length; i++)
                {
                    PHelpers.PrintToAll(Config.Maps[i]);
                }
            }
            
            if (Config.WorkshopMaps.Length > 0)
            {
                PHelpers.PrintToAll("---- Workshop maps ---- ");
                for (int i = 0; i < Config.WorkshopMaps.Length; i++)
                {
                    PHelpers.PrintToAll(Config.WorkshopMaps[i]);
                }
            }

            if (Config.WorkshopMaps.Length == 0 && Config.Maps.Length == 0)
            {
                PHelpers.PrintToAll("No maps are listed in config, please specify your maps in ...\\configs\\plugins\\ChangeLevelChat");
            }
        }

        [ConsoleCommand("css_modes", "List all gamemodes.")]
        [CommandHelper(whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
        public void OnListGamemodesCommand(CCSPlayerController? player, CommandInfo commandInfo)
        {
            PHelpers.Debug("Listing gamemodes...");
            if (!IsEnabled()) return;

            if (Config.GameModes.Length > 0)
            {
                PHelpers.PrintToAll("Avaivable presets: ");
                for (int i = 0; i < Config.GameModes.Length; i++)
                {
                    PHelpers.PrintToAll(Config.GameModes[i]);
                }
            }
            else
            {
                PHelpers.PrintToAll("No gamemodes are listed in config, please specify your execs in ...\\configs\\plugins\\ChangeLevelChat");
            }
        }

        [ConsoleCommand("css_changetoggle", "Toggle if users can use this plugin.")]
        [CommandHelper(whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
        [RequiresPermissions("@css/admin")]
        public void OnChangeToggle(CCSPlayerController? player, CommandInfo commandInfo)
        {
            Config.IsEnabled = !Config.IsEnabled;
            PHelpers.ReplyToCmd(commandInfo, Config.IsEnabled ? "This plugin is now enabled" : "This plugin is now disabled");
        }

        #endregion
    }
}

