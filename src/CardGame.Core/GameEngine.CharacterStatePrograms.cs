namespace CardGame.Core;
public enum CharacterStateContinuation
{
    Boundary,
    Program,
    CardUse,
    SkippedTurn,
    VirtualBasicCardUse,
    ChainedStateBasic,
    SoleIncomingRangeDraw = 1243,
    SkillInvocation = 38700,
    NativeCardEffectDraw = 27578,
    WenheCardEffect = 550,
    WumiaoFlood = 28510,
    DragonBoatCardEffect = 43000,
    GuanduCardEffect = 45000,
    KemingEcho = 39700
}

public sealed record CharacterStateChangeContext(long Id, long? ParentFrameId, int TargetSeat, SkillProgramTriggerWindow Window)
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public CharacterTurnedOverState? TurnedOver { get; init; }
}

public sealed record CharacterStateChangedEvent(CharacterStateChangeContext Change) : IGameEvent;
