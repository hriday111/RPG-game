namespace RpgGame.Logger;

/// <summary>
/// Default event-to-text formatter for the in-game log.
/// </summary>
public sealed class DefaultLogEventFormatter : ILogEventFormatter
{
    /// <inheritdoc />
    public string Format(GameLogEvent evt)
    {
        ArgumentNullException.ThrowIfNull(evt);

        return evt.Type switch
        {
            LogEventType.ItemPickedUp =>
                $"Player picked up {GetRequired<string>(evt, "ItemName")}.",
            LogEventType.ItemToEquipped =>
                $"Player equipped {GetRequired<string>(evt, "ItemName")} to {GetRequired<string>(evt, "Slot")}.",
            LogEventType.PlayerHealed =>
                $"Player healed {GetRequired<int>(evt, "Amount")} HP.",
            LogEventType.PlayerDamaged =>
                $"Player took {GetRequired<int>(evt, "Amount")} damage.",
            LogEventType.AttackDealt =>
                $"Player dealt {GetRequired<int>(evt, "Amount")} damage to {GetRequired<string>(evt, "Target")}.",
            LogEventType.EnemyDefeated =>
                $"{GetRequired<string>(evt, "EnemyName")} was defeated.",
            LogEventType.SessionStarted =>
                $"Game session started. Log file: {GetRequired<string>(evt, "LogFilePath")}",
            _ => evt.Type.ToString()
        };
    }

    private static T GetRequired<T>(GameLogEvent evt, string key)
    {
        if (!evt.Data.TryGetValue(key, out object? value))
            throw new InvalidOperationException($"Missing event key '{key}' for {evt.Type}.");

        if (value is T typed)
            return typed;

        throw new InvalidOperationException($"Event key '{key}' for {evt.Type} has invalid type.");
    }
}
