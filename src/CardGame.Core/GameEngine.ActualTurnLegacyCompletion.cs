namespace CardGame.Core;

// Completion-only normalization keeps legacy declaration, damage and recovery
// semantics intact. The exact owning use carries any delayed program return.
public sealed record LegacyActualUseCompletionCapturedEvent(long CardUseFrameId, int ActorSeat,
    CardKind EffectiveKind, long ProgramParentFrameId, CardConversionSource ProducerSource,
    string ProducerGameplayHash, int ProducerInstructionIndex, CardActionContext Action) : IGameEvent;

public sealed record LegacyDyingAlcoholCompletionReturn(long ProgramFrameId, int InstructionIndex,
    CardConversionSource ProducerSource);
