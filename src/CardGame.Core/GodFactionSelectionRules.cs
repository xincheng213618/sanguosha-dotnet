namespace CardGame.Core;

/// <summary>The existing selectable God-general factions, shared with scenario defaults.</summary>
public static class GodFactionSelectionRules
{
    public static IReadOnlyList<string> Choices { get; } =
        Array.AsReadOnly(new[] { "wei", "shu", "wu", "qun" });

    public static void ValidateDefault(string? factionId)
    {
        if (factionId is not null && !Choices.Contains(factionId, StringComparer.Ordinal))
            throw new ArgumentException("A default God faction must be one of the selectable God-general factions.", nameof(factionId));
    }
}
