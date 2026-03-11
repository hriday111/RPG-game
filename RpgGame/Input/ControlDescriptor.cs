using System;

namespace RpgGame.Input
{
    /// <summary>
    /// Holds information about a single key binding and a human
    /// readable description that can be shown in a help popup.
    /// </summary>
    public sealed class ControlDescriptor
    {
        public ConsoleKey Key { get; }
        public ConsoleModifiers Modifiers { get; }
        public string Description { get; }

        public ControlDescriptor(ConsoleKey key, ConsoleModifiers modifiers, string description)
        {
            Key = key;
            Modifiers = modifiers;
            Description = description;
        }

        /// <summary>
        /// Text suitable for display, e.g. "Shift+Q" or "W".
        /// </summary>
        public string DisplayText =>
            Modifiers == 0 ? Key.ToString() : $"{Modifiers}+{Key}";
    }
}