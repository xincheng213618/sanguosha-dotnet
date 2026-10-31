namespace CardGame.Core;

// 鬻爵 evidence: one scalar record per accepted invocation. The abolished slot,
// the card recipient and the given card id are all public; the 执笏 grant is
// rebuilt by replaying the accepted command, so the event carries no hidden ids
// beyond the given card's public hand-to-hand movement.
public sealed record ProgramYujueInvokedEvent(long FrameId, string SkillId, string BindingId,
    int OwnerSeat, string AbolishedSlot, int TargetSeat, int GivenCardId) : IGameEvent;
public sealed record ProgramYujueSlotPaidEvent(long FrameId, CardConversionSource Source,
    string GameplayHash, int InstructionIndex, string AbolishedSlot) : IGameEvent;
public sealed record ProgramYujueGiftPaidEvent(long FrameId, CardConversionSource Source,
    string GameplayHash, int InstructionIndex, int TargetSeat, int GivenCardId, int MovementSequence) : IGameEvent;

// Shared game-long damage bonus: arming evidence for one seat. Any content skill
// can arm its owner through this generic event; FinalizeAttackDamageAmount adds
// the armed amount to every damage the seat deals (chain propagation included),
// so the armed state is derived from committed history on cold recovery.
public sealed record ProgramGameDamageBonusArmedEvent(long FrameId, string SkillId, string BindingId,
    string SkillInstanceId, int OwnerSeat, int Amount) : IGameEvent;

public sealed partial record ProgramSkillFrame
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public YujuePendingState? YujuePending { get; init; }
}

// The suspension receipt of one accepted 鬻爵 invocation. AbolishedSlot is set
// once the slot prompt resolves; TargetSeat joins for the card-give prompt.
public enum YujuePendingStage { SlotPaid, GiftPaid }
public sealed record YujuePendingState(string AbolishedSlot, int? TargetSeat,
    CardConversionSource Source, string GameplayHash, int InstructionIndex,
    YujuePendingStage Stage = YujuePendingStage.SlotPaid, int? GivenCardId = null, int? GiftMovementSequence = null);

public sealed partial class GameEngine
{
}
