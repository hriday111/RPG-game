namespace RpgGame.Logger;

/// <summary>
/// Base class for log events with shared timestamp handling.
/// </summary>
public abstract class LogEventBase : ILogEvent
{
    /// <inheritdoc />
    public DateTime Timestamp { get; }

    /// <summary>
    /// Initializes the event with current local time.
    /// </summary>
    protected LogEventBase()
        : this(DateTime.Now)
    {
    }

    /// <summary>
    /// Initializes the event with an explicit timestamp.
    /// </summary>
    /// <param name="timestamp">Event timestamp.</param>
    protected LogEventBase(DateTime timestamp)
    {
        Timestamp = timestamp;
    }

    /// <inheritdoc />
    public abstract string ToMessage();
}

/// <summary>
/// Event recorded when a game session starts.
/// </summary>
public sealed class SessionStartedLogEvent : LogEventBase
{
    /// <summary>
    /// Gets the session log file path.
    /// </summary>
    public string LogFilePath { get; }

    /// <summary>
    /// Initializes a new session-start event.
    /// </summary>
    public SessionStartedLogEvent(string logFilePath)
    {
        LogFilePath = logFilePath ?? throw new ArgumentNullException(nameof(logFilePath));
    }

    /// <inheritdoc />
    public override string ToMessage() => $"Game session started. Log file: {LogFilePath}";
}

/// <summary>
/// Event recorded when the player picks up an item.
/// </summary>
public sealed class ItemPickedUpLogEvent : LogEventBase
{
    /// <summary>
    /// Gets the picked item name.
    /// </summary>
    public string ItemName { get; }

    /// <summary>
    /// Initializes a new pickup event.
    /// </summary>
    public ItemPickedUpLogEvent(string itemName)
    {
        ItemName = itemName ?? throw new ArgumentNullException(nameof(itemName));
    }

    /// <inheritdoc />
    public override string ToMessage() => $"Player picked up {ItemName}.";
}

/// <summary>
/// Event recorded when the player equips an item to a slot.
/// </summary>
public sealed class ItemEquippedLogEvent : LogEventBase
{
    /// <summary>
    /// Gets the equipped item name.
    /// </summary>
    public string ItemName { get; }

    /// <summary>
    /// Gets the equipment slot label.
    /// </summary>
    public string Slot { get; }

    /// <summary>
    /// Initializes a new equip event.
    /// </summary>
    public ItemEquippedLogEvent(string itemName, string slot)
    {
        ItemName = itemName ?? throw new ArgumentNullException(nameof(itemName));
        Slot = slot ?? throw new ArgumentNullException(nameof(slot));
    }

    /// <inheritdoc />
    public override string ToMessage() => $"Player equipped {ItemName} to {Slot}.";
}

/// <summary>
/// Event recorded when the player deals damage.
/// </summary>
public sealed class AttackDealtLogEvent : LogEventBase
{
    /// <summary>
    /// Gets the damage amount dealt.
    /// </summary>
    public int Amount { get; }

    /// <summary>
    /// Gets the target label.
    /// </summary>
    public string Target { get; }

    /// <summary>
    /// Initializes a new damage event.
    /// </summary>
    public AttackDealtLogEvent(int amount, string target)
    {
        Amount = amount;
        Target = target ?? throw new ArgumentNullException(nameof(target));
    }

    /// <inheritdoc />
    public override string ToMessage() => $"Player dealt {Amount} damage to {Target}.";
}

/// <summary>
/// Event recorded when an enemy is defeated.
/// </summary>
public sealed class EnemyDefeatedLogEvent : LogEventBase
{
    /// <summary>
    /// Gets defeated enemy name.
    /// </summary>
    public string EnemyName { get; }

    /// <summary>
    /// Initializes a new enemy-defeated event.
    /// </summary>
    public EnemyDefeatedLogEvent(string enemyName)
    {
        EnemyName = enemyName ?? throw new ArgumentNullException(nameof(enemyName));
    }

    /// <inheritdoc />
    public override string ToMessage() => $"{EnemyName} was defeated.";
}

/// <summary>
/// Event recorded when the player consumes a potion (health restored on pickup).
/// </summary>
public sealed class PotionConsumedLogEvent : LogEventBase
{
    /// <summary>
    /// Gets the HP restored by consuming the potion.
    /// </summary>
    public int AmountRestored { get; }

    /// <summary>
    /// Initializes a new potion-consumed event.
    /// </summary>
    /// <param name="amountRestored">Health points restored.</param>
    public PotionConsumedLogEvent(int amountRestored)
    {
        if (amountRestored <= 0)
            throw new ArgumentOutOfRangeException(nameof(amountRestored), "Restored amount must be positive.");

        AmountRestored = amountRestored;
    }

    /// <inheritdoc />
    public override string ToMessage() =>
        $"Player consumed a potion and restored {AmountRestored} HP.";
}
