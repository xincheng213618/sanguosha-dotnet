#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramWindowRuntimeServicesRuntime
{
    TurnPhase IProgramWindowReturnQueries.Phase => _coreActualTurnProgressPhaseAccess._phase;

    DamageTriggerWindowFrame? IProgramWindowReturnQueries.ActiveDamageTrigger => _coreDamageCursorRuntimeOperations.ActiveDamageTrigger;

    bool IProgramWindowReturnQueries.IsCurrentYanPuDyingExitEdge() => _characterYanPuOlOperations.IsYanPuDyingExitEdge(_resolutionStackOperations._resolutionStack.Count - 1);
    bool IProgramWindowReturnQueries.IsWushuangDyingExitReturn(ProgramSkillFrame frame) => _coreWushuangDyingExitOperations.IsWushuangDyingExitReturn(frame);
    bool IProgramWindowReturnQueries.IsHappyGuanYuDyingExitReturn(ProgramSkillFrame frame) => _characterGuanYuHappyGodOperations.IsHappyGuanYuDyingExitReturn(frame);
    bool IProgramWindowReturnQueries.IsShanheYongzhanDyingExitReturn(ProgramSkillFrame frame) => _coreShanheYongzhanOperations.IsShanheYongzhanDyingExitReturn(frame);
    bool IProgramWindowReturnQueries.MountObserverCandidateMatches(ProgramSkillFrame frame, ProgramTriggerCandidate candidate) => RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver.MountObserverCandidateMatches(frame, candidate);
    SkillProgramTrigger IProgramWindowReturnQueries.GetProgramTrigger(ProgramSkillFrame frame) => _coreProgramLifecycleOperations.GetProgramTrigger(frame);
    bool IProgramWindowReturnQueries.ValidBeiMiHuPreludeLifecycleWindow(ProgramLifecycleTriggerWindowFrame frame) => _characterBeiMiHuOlOperations.ValidBeiMiHuPreludeLifecycleWindow(frame);
    bool IProgramWindowReturnQueries.IsMingchaNormalDrawReplacement(ProgramSkillFrame frame) => _characterRuanHuiOlOperations.IsMingchaNormalDrawReplacement(frame);
    bool IProgramWindowReturnQueries.IsPhaseHandDebtResolver(ProgramTriggerCandidate candidate) => _corePhaseHandSeizureOperations.IsPhaseHandDebtResolver(candidate);
    void IProgramWindowNativeReturns.CompletePindianRevealBinding(ProgramSkillFrame frame) => _corePindianRevealWindowsOperations.CompletePindianRevealBinding(frame);
    void IProgramWindowNativeReturns.CompleteSlashTargetPenaltyBinding(ProgramSkillFrame frame) => _coreSlashTargetPenaltyWindowsOperations.CompleteSlashTargetPenaltyBinding(frame);
    void IProgramWindowNativeReturns.CompleteSlashTargetBenefitBinding(ProgramSkillFrame frame) => _coreSlashTargetBenefitWindowsOperations.CompleteSlashTargetBenefitBinding(frame);
    void IProgramWindowNativeReturns.CompleteActualUseTargetBinding(ProgramSkillFrame frame, bool completed) => _coreActualUseTargetProgramsOperations.CompleteActualUseTargetBinding(frame, completed);
    void IProgramWindowNativeReturns.CompleteForeignActualTurnStartBinding(ProgramSkillFrame frame) => _coreForeignTurnContestsOperations.CompleteForeignActualTurnStartBinding(frame);
    void IProgramWindowNativeReturns.CompleteDyingProgramBinding(ProgramSkillFrame frame, bool completed) => _coreProgramLifecycleOperations.CompleteDyingProgramBinding(frame, completed);
    void IProgramWindowNativeReturns.AdvanceDamageTriggerCandidate(DamageTriggerWindowFrame frame) => _coreNativeDamageWindowsOperations.AdvanceDamageTriggerCandidate(frame);
    void IProgramWindowNativeReturns.CompleteCardEffectCandidate(ProgramSkillWindowContext context) => _coreCardEffectProgramsOperations.CompleteCardEffectCandidate(context);
    void IProgramWindowNativeReturns.RestoreCompletedUseAttackAfterIssuedChild(ProgramSkillFrame frame, ProgramCardTriggerWindowFrame parent) => _coreCompletedUseChildReturnsOperations.RestoreCompletedUseAttackAfterIssuedChild(frame, parent);
    void IProgramWindowNativeReturns.CompleteProgramJudgmentReplacementBinding(ProgramSkillFrame frame, bool completed) => _coreJudgmentReplacementProgramsOperations.CompleteProgramJudgmentReplacementBinding(frame, completed);
}
