using RpgGame.Character;
using RpgGame.Core;
using Xunit;

namespace RpgGame.Tests;

public class PlayerTests
{
    [Fact]
    public void RestoreHealth_ShouldCapAt100()
    {
        // Arrange
        var player = new Player(new Position(0, 0));
        player.TakeDamage(10); // Assume it starts at some value, let's check Player ctor or just use it.

        // Act
        player.RestoreHealth(150);

        // Assert
        Assert.Equal(100, player.Health);
    }

    [Fact]
    public void TakeDamage_ShouldNotGoBelow0()
    {
        // Arrange
        var player = new Player(new Position(0, 0));

        // Act
        player.TakeDamage(200);

        // Assert
        Assert.Equal(0, player.Health);
    }

    [Fact]
    public void AddCoins_ShouldIncreaseCount()
    {
        // Arrange
        var player = new Player(new Position(0, 0));
        int initialCoins = player.Coins;

        // Act
        player.AddCoins(50);

        // Assert
        Assert.Equal(initialCoins + 50, player.Coins);
    }
}
