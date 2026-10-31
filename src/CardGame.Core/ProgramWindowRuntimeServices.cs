namespace CardGame.Core;

internal interface IProgramWindowCursors : IProgramRuntimeOperations
{
    void AdvanceProgramLifecycleCursor(ProgramLifecycleTriggerWindowFrame frame);
    void AdvancePlayPhaseStartingCursor(PlayPhaseStartingBoundaryFrame frame);
    void AdvanceTurnEndingBoundaryCursor(TurnEndingBoundaryFrame frame);
    void AdvanceCardsMovedProgramCursor(CardsMovedTriggerWindowFrame frame);
    void AdvanceHpChangedProgramCursor(HpChangedTriggerWindowFrame frame);
    void AdvanceOwnerDiedProgramCursor(ProgramDeathTriggerWindowFrame frame);
    void AdvanceBeforeDamageProgramCandidate(BeforeDamageProgramWindowFrame frame, bool activated, bool completed);
    void AdvanceProgramCardCandidate(ProgramCardTriggerWindowFrame frame);
    void AdvanceProgramJudgmentCandidate(ProgramJudgmentTriggerWindowFrame frame);
}

internal interface IProgramWindowReturnQueries : IProgramSelectionQuery
{
    TurnPhase Phase { get; }
    DamageTriggerWindowFrame? ActiveDamageTrigger { get; }
    bool IsCurrentYanPuDyingExitEdge();
    bool IsWushuangDyingExitReturn(ProgramSkillFrame frame);
    bool IsHappyGuanYuDyingExitReturn(ProgramSkillFrame frame);
    bool IsShanheYongzhanDyingExitReturn(ProgramSkillFrame frame);
    bool MountObserverCandidateMatches(ProgramSkillFrame frame, ProgramTriggerCandidate candidate);
    SkillProgramTrigger GetProgramTrigger(ProgramSkillFrame frame);
    bool ValidBeiMiHuPreludeLifecycleWindow(ProgramLifecycleTriggerWindowFrame frame);
    bool IsMingchaNormalDrawReplacement(ProgramSkillFrame frame);
    bool IsPhaseHandDebtResolver(ProgramTriggerCandidate candidate);
}

// These are returns into other native mechanisms. Parent selection and cursor
// ownership stay in ProgramWindowReturnRuntime, rather than in these adapters.
internal interface IProgramWindowNativeReturns : IProgramRuntimeOperations
{
    void CompletePindianRevealBinding(ProgramSkillFrame frame);
    void CompleteSlashTargetPenaltyBinding(ProgramSkillFrame frame);
    void CompleteSlashTargetBenefitBinding(ProgramSkillFrame frame);
    void CompleteActualUseTargetBinding(ProgramSkillFrame frame, bool completed);
    void CompleteForeignActualTurnStartBinding(ProgramSkillFrame frame);
    void CompleteDyingProgramBinding(ProgramSkillFrame frame, bool completed);
    void AdvanceDamageTriggerCandidate(DamageTriggerWindowFrame frame);
    void CompleteCardEffectCandidate(ProgramSkillWindowContext context);
    void RestoreCompletedUseAttackAfterIssuedChild(ProgramSkillFrame frame, ProgramCardTriggerWindowFrame parent);
    void CompleteProgramJudgmentReplacementBinding(ProgramSkillFrame frame, bool completed);
}
