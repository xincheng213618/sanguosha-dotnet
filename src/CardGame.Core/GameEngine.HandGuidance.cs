namespace CardGame.Core;

public enum HandGuidanceReason
{
    Playable, ConversionOnly, ResponseOnly, HealthFull, SlashLimitReached,
    AlcoholAlreadyActive, NoLegalTarget, WaitingForTurn, ResolveCurrentPrompt,
    SelectDiscard, GameFinished
}

/// <summary>Read-only guidance for one card owned by the local human, without opponent secrets.</summary>
public sealed record HandCardGuidance(int CardId, bool CanPlay, HandGuidanceReason Reason, string Message);
