namespace CardGame.Core;

public sealed record HpLossSlashSelection(CardConversionSource PolicySource, string PolicyHash, int FrozenMaximum,
    CardKind OutputKind, long? CardUseFrameId = null);
public sealed record HpLossMaterialSlashReturn(long CardUseFrameId, long ActionId, CardConversionSource PolicySource,
    string PolicyHash, int FrozenMaximum, long? ParentProgramFrameId, string? ParentGameplayHash,
    string? ParentSkillInstanceId, int ParentInstructionIndex, bool IsZhangba, long PaidFirst, long PaidLast = 0);
public sealed record HpLossMaterialSlashIssuedEvent(long CardUseFrameId, long ActionId, CardConversionSource PolicySource,
    string PolicyHash, int FrozenMaximum, long? ParentProgramFrameId, bool IsZhangba) : IGameEvent;
