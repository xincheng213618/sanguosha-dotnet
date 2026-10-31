namespace CardGame.Core;

internal interface IProgramDrawFacts : IProgramSelectionQuery
{
    bool HasHpValue(int seat);
    int MaxHp(int seat);
    string? EffectiveFaction(int seat);
    int AttackRange(int seat);
    long PairBenefitSequence { get; }
    int TurnOwnerDiscardPhaseHandDiscardCount { get; }
    int CategoryTargetTurnUsage(ProgramSkillFrame frame);
    int OwnerLostHp(ProgramSkillFrame frame);
    int CurrentTurnCategoryDrawCount(ProgramSkillFrame frame, int targetSeat);
    int ActualTurnDeathDrawCount(ProgramSkillFrame frame, int targetSeat);
    int CommittedMijiCount(ProgramSkillFrame frame);
}

// Native producers retain their physical draw and original paid receipts. The
// program runtime controls when each operation is called and binds its result.
internal interface IProgramDrawOperations : IProgramRuntimeOperations
{
    void Cancel(ProgramSkillFrame frame, string reason);
    IReadOnlyList<int> DrawCards(int seat, int count, bool log, CardMoveReason reason);
    void CaptureFixedRecipientDraw(long frameId, int targetSeat, CardMoveReason reason, long sequenceBefore, int actualCount);
    int FreezeNormalDrawBase(int seat);
}

public sealed partial class GameEngine
{
}
