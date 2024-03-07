using CounterStrikeSharp.API.Core;
using System.Text.Json.Serialization;

namespace ChangeLevelChat
{
    public sealed class Config : BasePluginConfig
    {
        [JsonPropertyName("isEnabled")]
        public bool IsEnabled { get; set; } = true;

        [JsonPropertyName("Default Game Mode")]
        public string? DefaultGameMode { get; set; } = null;

        [JsonPropertyName("Game Modes")]
        public string[] GameModes { get; set; } = Array.Empty<string>();

        [JsonPropertyName("Maps")]
        public string[] Maps { get; set; } = new string[] {"ar_baggage", "ar_shoots", "cs_italy", "cs_office", "de_ancient", "de_anubis", "de_dust2", "de_inferno", "de_mirage", "de_nuke", "de_overpass", "de_vertigo"};

        [JsonPropertyName("Workshop Maps")]
        public string[] WorkshopMaps { get; set; } = Array.Empty<string>();

        [JsonPropertyName("Log Level")]
        public string LogLevel { get; set; } = "Info";

        [JsonPropertyName("ConfigVersion")]
        public override int Version { get; set; } = 1;
    }
}