using System.Globalization;

namespace RpgGame.Core;

/// <summary>
/// Loads <see cref="GameConfig"/> values from an INI file.
/// </summary>
/// <remarks>
/// Supported format uses sections such as <c>[Window]</c> and key/value pairs
/// in the form <c>Key=Value</c>.
/// </remarks>
public static class IniConfigLoader
{
    /// <summary>
    /// Loads, parses, and validates the game configuration from disk.
    /// </summary>
    /// <param name="path">Absolute or relative path to the INI file.</param>
    /// <returns>A validated <see cref="GameConfig"/> instance.</returns>
    /// <exception cref="FileNotFoundException">
    /// Thrown when the specified configuration file does not exist.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when required sections/keys are missing or values are invalid.
    /// </exception>
    public static GameConfig Load(string path)
    {
        if (!File.Exists(path)) { throw new FileNotFoundException($"Config file not found: {path}"); }

        var sections = ParseIni(path);

        int windowWidth = ReadInt(sections, "Window", "Width");
        int windowHeight = ReadInt(sections, "Window", "Height");
        int targetFPS = ReadInt(sections, "Window", "TargetFPS");
        int spawnX = ReadInt(sections, "Player", "DefaultSpawnX");
        int spawnY = ReadInt(sections, "Player", "DefaultSpawnY");
        string playerName = ReadString(sections, "Player", "Name");
        int sidebarWidth = ReadInt(sections, "Sidebar", "Width");
        string logDirectory = ReadString(sections, "Logging", "Directory");
        Validate(windowWidth, windowHeight, targetFPS, spawnX, spawnY, sidebarWidth);
        return new GameConfig
        {
            WindowWidth = windowWidth,
            WindowHeight = windowHeight,
            TargetFPS = targetFPS,
            DefaultSpawnX = spawnX,
            DefaultSpawnY = spawnY,
            SidebarWidth = sidebarWidth,
            PlayerName = playerName,
            LogDirectory = logDirectory
        };
    }

    /// <summary>
    /// Parses INI file content into a nested section/key dictionary.
    /// </summary>
    /// <param name="path">Path to the INI file.</param>
    /// <returns>
    /// A dictionary of section names mapped to key/value dictionaries.
    /// </returns>
    private static Dictionary<string, Dictionary<string, string>> ParseIni(string path)
    {
        var result = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
        string currentSection = string.Empty;
        foreach (string raw in File.ReadLines(path))
        {
            string line = raw.Trim();

            if (line.Length == 0 || line.StartsWith(';') || line.StartsWith('#'))
            {
                continue;
            }

            if (line.StartsWith('[') && line.EndsWith(']'))
            {
                currentSection = line[1..^1].Trim();
                if (!result.ContainsKey(currentSection))
                    result[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                continue;
            }
            int eq = line.IndexOf('=');
            if (eq <= 0)
                continue;
            if (string.IsNullOrWhiteSpace(currentSection))
                throw new InvalidOperationException($"Key outside section: {line}");
            string key = line[..eq].Trim();
            string value = line[(eq + 1)..].Trim();
            result[currentSection][key] = value;
        }
        return result;
    }

    /// <summary>
    /// Reads and converts an INI value to an integer.
    /// </summary>
    /// <param name="sections">Parsed section/key dictionary.</param>
    /// <param name="section">Section name.</param>
    /// <param name="key">Key name in the section.</param>
    /// <returns>The parsed integer value.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the value is missing or not a valid integer.
    /// </exception>
    private static int ReadInt(
        Dictionary<string, Dictionary<string, string>> sections,
        string section,
        string key)
    {
        string raw = ReadString(sections, section, key);
        if (!int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
        { throw new InvalidOperationException($"Invalid integer for [{section}] {key}: {raw}"); }
        return value;
    }

    /// <summary>
    /// Reads a required non-empty string value from the parsed INI structure.
    /// </summary>
    /// <param name="sections">Parsed section/key dictionary.</param>
    /// <param name="section">Section name.</param>
    /// <param name="key">Key name in the section.</param>
    /// <returns>The string value found in the INI file.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the section or key is missing, or value is empty.
    /// </exception>
    private static string ReadString(
        Dictionary<string, Dictionary<string, string>> sections,
        string section,
        string key)
    {
        if (!sections.TryGetValue(section, out var kv))
        { throw new InvalidOperationException($"Missing section: [{section}]"); }
        if (!kv.TryGetValue(key, out string? value) || string.IsNullOrWhiteSpace(value))
        { throw new InvalidOperationException($"Missing key: [{section}] {key}"); }
        return value;
    }

    /// <summary>
    /// Validates core numeric constraints for runtime configuration.
    /// </summary>
    /// <param name="width">Configured window width.</param>
    /// <param name="height">Configured window height.</param>
    /// <param name="fps">Configured target frames per second.</param>
    /// <param name="spawnX">Configured player spawn X coordinate.</param>
    /// <param name="spawnY">Configured player spawn Y coordinate.</param>
    /// <param name="sidebarWidth">Configured sidebar width.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when one or more values are outside accepted ranges.
    /// </exception>
    private static void Validate(
        int width, int height, int fps, int spawnX, int spawnY, int sidebarWidth)
    {
        if (width <= 0 || height <= 0)
        { throw new InvalidOperationException("Window size must be > 0."); }
        if (fps <= 0)
        { throw new InvalidOperationException("TargetFPS must be > 0."); }
        if (sidebarWidth <= 0)
        { throw new InvalidOperationException("Sidebar width must be > 0."); }
        if (spawnX < 0 || spawnX >= width || spawnY < 0 || spawnY >= height)
        { throw new InvalidOperationException("Player spawn must be inside window bounds."); }
    }
}
