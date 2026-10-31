namespace CardGame.Core;

internal interface IProgramPhaseQueries : IProgramSelectionQuery
{
    TurnPhase Phase { get; }
}

internal interface IProgramPhaseSchedulingFacts : IProgramSelectionQuery
{
    Winner Winner { get; }
    bool MatchesAfterTurnEndedChild(DeferredTurnEndFrame parent, ProgramSkillFrame child, ProgramSkillWindowContext context);
    bool HasRuntimeSkillInstance(int seat, string skillId, string instanceId);
    bool IsReturningGrantedEntityPhase(ProgramSkillFrame frame);
}

// This bridge reads and writes the original schedule. It neither retains a
// second schedule nor owns another pending continuation.
internal interface IProgramPhaseScheduleState : IProgramRuntimeOperations
{
    ProgramPhaseSchedule? Current { get; set; }
    void RestorePhase(TurnPhase phase);
}

// The scheduler controls the order. Native phase entry, draw replacement,
// draw-base capture and character-specific consumption stay with their owners.
internal interface IProgramPhaseNativeEntries : IProgramRuntimeOperations
{
    void EnterPlayPhase(int seat);
    bool TryBeginDrawPhaseProgramWindow(int seat);
    void ConsumeDuFuRenDrawPhase(int seat);
    void ConsumeChengGongYingDraw(int seat);
    void DrawNormalCards(int seat);
    void CompleteTurnStartAfterDraw(int seat);
}

// Token restoration, granted-entity receipts, expiry and the character-specific
// typed return remain native mechanisms below the scheduling protocol.
internal interface IProgramPhaseNativeReturns : IProgramRuntimeOperations
{
    bool TryResumeZhangShiPingHongjiPhase();
    bool TryCancelZhangShiPingHongjiPhase();
    ProgramSkillFrame FreezeGrantedEntityPhaseEnd(ProgramSkillFrame frame);
    void ResumeZhangYiDianjunExtraPlay(long frameId);
    void CompleteDetachedProgramBinding(ProgramSkillFrame frame, bool completed);
    void ExpireActualTurnEndEffects(int seat);
    void ExpirePlayPhaseSkillGrantsForEndedTurn(int turnNumber);
    void ExpireActualTurnSkillGrants(int turnNumber, int seat);
}

internal interface IProgramPhasePipelineHooks : IProgramRuntimeOperations
{
    bool ResumeScheduledPhase();
    bool CancelScheduledPhase();
    bool ContinueDetachedProgramBinding(ProgramSkillFrame frame);
}

public sealed partial class GameEngine
{
}
