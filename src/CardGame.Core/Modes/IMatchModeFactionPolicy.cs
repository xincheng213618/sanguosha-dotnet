namespace CardGame.Core;

/// <summary>Already resolved native declarations and library facts; precedence belongs to the mode.</summary>
public sealed record ModeFactionFacts(string? TeamId, string? NationalFactionId, string? NativeOverride,
    string? LibraryFaction, string? ChosenFaction, string? PrintedFaction);

public interface IMatchModeFactionPolicy : IGameModeFeature
{
    string? EffectiveFaction(ModeFactionFacts facts);
    string? ScenarioGodFaction(string? printedFaction);
}
