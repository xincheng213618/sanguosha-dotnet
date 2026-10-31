namespace CardGame.Core;

internal interface IActualPhaseEndQueries : IProgramSelectionQuery
{
    bool Tracks { get; }
    bool IsExactActualPhaseEndCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
}

internal interface IActualPhaseEndOperations : IProgramRuntimeOperations
{
    void ConfirmActualDiscardPhaseExecution();
    bool TryBeginActualPhaseEnd(ActualDiscardRecoveryPhaseKind kind, ActualPhaseEndContinuation continuation,
        int delayedEffects = 0, bool afterNormalDrawProgramsCompleted = false,
        bool drawPhaseEndedProgramsCompleted = false, bool actualDrawCompletionCompleted = false,
        long? parentFrameId = null);
    void ReturnActualPhaseEndWindow(ProgramLifecycleTriggerWindowFrame window);
}

internal interface IActualPhaseEndFacts : IProgramSelectionQuery
{
    Winner Winner { get; }
    bool HasLishiCapability { get; }
    bool HasMouSimaShi { get; }
    bool HasMouHuCheEr { get; }
    bool ModeTracksActualPhaseEnds { get; }
    bool HasOlLuShi { get; }
}

// Domain owners qualify their own issued entitlements. The phase-end runtime
// owns their combined order, frozen roster, late additions and exact cursor.
internal interface IActualPhaseEndIssuedItems : IProgramSelectionQuery
{
    IReadOnlyList<ActualPhaseEndItem> Qingya(ActualDiscardRecoveryPhaseKey phase);
    IReadOnlyList<ActualPhaseEndItem> MouSimaShi(ActualDiscardRecoveryPhaseKey phase);
    IReadOnlyList<ActualPhaseEndItem> ShiHuangZu(ActualDiscardRecoveryPhaseKey phase);
    IReadOnlyList<ActualPhaseEndItem> ShiLuYuSheng(ActualDiscardRecoveryPhaseKey phase);
}

// These enter native phase producers only after the runtime has selected and
// validated the original continuation. The private delayed-effect enum never
// crosses this protocol; the owning native producer maps the original integer.
internal interface IActualPhaseEndNativeReturns : IProgramRuntimeOperations
{
    void BeginTurnStartAfterProgramLifecycle(int actorSeat);
    void CompleteTurnStart(int actorSeat, int delayedEffects);
    void CompleteTurnStartAfterDraw(int actorSeat, int delayedEffects,
        bool afterNormalDrawProgramsCompleted, bool drawPhaseEndedProgramsCompleted, bool actualDrawCompletionCompleted);
    void CompletePlayPhaseAfterProgramWindow();
    void EndTurn();
    void FinalizeEndTurn(int actorSeat);
    void AdvanceRulesAndPublishState();
    bool ResumeScheduledProgramPhase();
}
