namespace CardGame.Core;

public sealed record CurrentEnhancementSlashRedirectedEvent(long ProgramFrameId, long CardUseFrameId,
    long ActionId, CardConversionSource Source, string GameplayHash, int InstructionIndex, int TargetIndex,
    int PreviousTargetSeat, int NewTargetSeat, IReadOnlyList<int> BeforeTargets,
    IReadOnlyList<int> AfterTargets, long WindowFrameId) : IGameEvent;
