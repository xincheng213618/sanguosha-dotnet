namespace CardGame.Core;

/// <summary>The public, immutable face-state transition that opened a turned-over window.</summary>
public sealed record CharacterTurnedOverState(bool WasFaceDown, bool IsFaceDown);
