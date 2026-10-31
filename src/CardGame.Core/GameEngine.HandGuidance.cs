namespace CardGame.Core;

public enum HandGuidanceReason
{
    Playable, ConversionOnly, ResponseOnly, HealthFull, SlashLimitReached,
    AlcoholAlreadyActive, NoLegalTarget, WaitingForTurn, ResolveCurrentPrompt,
    SelectDiscard, GameFinished
}

/// <summary>Read-only guidance for one card owned by the local human, without opponent secrets.</summary>
public sealed record HandCardGuidance(int CardId, bool CanPlay, HandGuidanceReason Reason, string Message);

public sealed partial class GameEngine
{
    /// <summary>Builds both UI queries together without enumerating legal actions twice.</summary>
    public HumanActionView GetHumanActionView()
=>GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreGameEngineHandGuidanceRuntime>().GetHumanActionView();

    /// <summary>
    /// Explains the current human hand. Legal actions remain authoritative;
    /// this query does not advance a frame, consume randomness or publish events.
    /// </summary>
    public IReadOnlyList<HandCardGuidance> GetHumanHandGuidance() =>GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreHandCardGuidanceProjectionRuntime>().GetHumanHandGuidance();
}
