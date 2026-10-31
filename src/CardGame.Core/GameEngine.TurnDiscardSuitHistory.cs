namespace CardGame.Core;

/// <summary>Opt-in scalar history; contains no card identities or private locations.</summary>
public sealed record TurnDiscardSuitMaskChangedEvent(int TurnNumber, int SuitMask) : IGameEvent;
