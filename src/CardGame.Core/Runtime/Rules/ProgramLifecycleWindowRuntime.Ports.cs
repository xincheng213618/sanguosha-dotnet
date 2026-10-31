namespace CardGame.Core;

internal interface IProgramLifecycleFacts : IProgramSelectionQuery
{
    int? DyingKillerSeat { get; }
    bool HasAttributedEventOperations { get; }
    IReadOnlyDictionary<PlayerMarkerKind, int> MarkerCounts(int seat);
    bool IsExactHellAppearanceWindow(ProgramLifecycleTriggerWindowFrame frame);
}

// Rule-owned evidence decoration stays with each native producer. These reads
// never replace the lifecycle cursor or retain another pending context.
internal interface IProgramLifecycleContexts : IProgramSelectionQuery
{
    bool IsIssuedDiciChainTrigger(SkillProgramTrigger trigger);
    ProgramSkillWindowContext IssuedDiciChain(ProgramLifecycleTriggerWindowFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
    ProgramSkillWindowContext IdentityAppearance(ProgramLifecycleTriggerWindowFrame frame, ProgramTriggerCandidate candidate);
    ProgramSkillWindowContext QingzhongEnding(ProgramLifecycleTriggerWindowFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
    ProgramSkillWindowContext WeiluEnding(ProgramLifecycleTriggerWindowFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
    ProgramSkillWindowContext XuRongDying(ProgramSkillWindowContext context);
    ProgramSkillWindowContext ZhuijiEnding(ProgramLifecycleTriggerWindowFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
    ProgramSkillWindowContext PhaseNamePrediction(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
    ProgramSkillWindowContext NativeDraw(ProgramLifecycleTriggerWindowFrame frame, ProgramSkillWindowContext context);
}

// Each return enters the existing native owner with the original typed frame.
// The independent window runtime owns dispatch and exact parent selection.
internal interface IProgramLifecycleReturns : IProgramRuntimeOperations
{
    void DrawPhaseObligation(ProgramLifecycleTriggerWindowFrame frame);
    bool PipelineCompleted(ProgramLifecycleTriggerWindowFrame frame);
    void NativeDrawAttempt(ProgramLifecycleTriggerWindowFrame frame);
    void RoundEnding(ProgramLifecycleTriggerWindowFrame frame);
    void YanPuDyingExit(ProgramLifecycleTriggerWindowFrame frame);
    void AfterForeignActualContests(int seat);
    void DyingEntry(ProgramLifecycleTriggerWindowFrame frame);
    void AfterHengye(int seat);
    bool ZhouQunPaidPreparation(ProgramLifecycleTriggerWindowFrame frame);
    bool DuFuRenPaidPreparation(ProgramLifecycleTriggerWindowFrame frame);
    bool ChengGongYingPaidPreparation(ProgramLifecycleTriggerWindowFrame frame);
    void NormalTurnStart(int seat);
    void IdentityAppearance(ProgramLifecycleTriggerWindowFrame frame);
    void BeiMiHuPrelude(ProgramLifecycleTriggerWindowFrame frame);
    void AfterRoundPrograms(int seat);
    void DrawPhase(ProgramLifecycleTriggerWindowFrame frame, bool actualPhaseSubstituted);
    void PlayPhase();
    void EndTurn();
    void JudgmentStarting(ProgramLifecycleTriggerWindowFrame frame);
    void CharacterStateChange(ProgramLifecycleTriggerWindowFrame frame);
    void DrawPhaseEnded(int seat, int delayedEffects);
    void AfterNormalDraw(int seat);
    void DiscardPhase(int seat);
}
