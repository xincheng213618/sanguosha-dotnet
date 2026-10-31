#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramWindowRuntimeServicesRuntime
{
    TurnPhase global::CardGame.Core.IProgramWindowReturnQueries.Phase => _coreActualTurnProgressPhaseAccess._phase;

    DamageTriggerWindowFrame? global::CardGame.Core.IProgramWindowReturnQueries.ActiveDamageTrigger => _coreDamageCursorRuntimeOperations.ActiveDamageTrigger;

    bool global::CardGame.Core.IProgramWindowReturnQueries.IsCurrentYanPuDyingExitEdge() => _characterYanPuOlOperations.IsYanPuDyingExitEdge(_resolutionStackOperations._resolutionStack.Count - 1);
    bool global::CardGame.Core.IProgramWindowReturnQueries.IsWushuangDyingExitReturn(ProgramSkillFrame frame) => _coreWushuangDyingExitOperations.IsWushuangDyingExitReturn(frame);
    bool global::CardGame.Core.IProgramWindowReturnQueries.IsHappyGuanYuDyingExitReturn(ProgramSkillFrame frame) => _characterGuanYuHappyGodOperations.IsHappyGuanYuDyingExitReturn(frame);
    bool global::CardGame.Core.IProgramWindowReturnQueries.IsShanheYongzhanDyingExitReturn(ProgramSkillFrame frame) => _coreShanheYongzhanOperations.IsShanheYongzhanDyingExitReturn(frame);
    bool global::CardGame.Core.IProgramWindowReturnQueries.MountObserverCandidateMatches(ProgramSkillFrame frame, ProgramTriggerCandidate candidate) => RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver.MountObserverCandidateMatches(frame, candidate);
    SkillProgramTrigger global::CardGame.Core.IProgramWindowReturnQueries.GetProgramTrigger(ProgramSkillFrame frame) => _coreProgramLifecycleOperations.GetProgramTrigger(frame);
    bool global::CardGame.Core.IProgramWindowReturnQueries.ValidBeiMiHuPreludeLifecycleWindow(ProgramLifecycleTriggerWindowFrame frame) => _characterBeiMiHuOlOperations.ValidBeiMiHuPreludeLifecycleWindow(frame);
    bool global::CardGame.Core.IProgramWindowReturnQueries.IsMingchaNormalDrawReplacement(ProgramSkillFrame frame) => _characterRuanHuiOlOperations.IsMingchaNormalDrawReplacement(frame);
    bool global::CardGame.Core.IProgramWindowReturnQueries.IsPhaseHandDebtResolver(ProgramTriggerCandidate candidate) => _corePhaseHandSeizureOperations.IsPhaseHandDebtResolver(candidate);
    void global::CardGame.Core.IProgramWindowNativeReturns.CompletePindianRevealBinding(ProgramSkillFrame frame) => _corePindianRevealWindowsOperations.CompletePindianRevealBinding(frame);
    void global::CardGame.Core.IProgramWindowNativeReturns.CompleteSlashTargetPenaltyBinding(ProgramSkillFrame frame) => _coreSlashTargetPenaltyWindowsOperations.CompleteSlashTargetPenaltyBinding(frame);
    void global::CardGame.Core.IProgramWindowNativeReturns.CompleteSlashTargetBenefitBinding(ProgramSkillFrame frame) => _coreSlashTargetBenefitWindowsOperations.CompleteSlashTargetBenefitBinding(frame);
    void global::CardGame.Core.IProgramWindowNativeReturns.CompleteActualUseTargetBinding(ProgramSkillFrame frame, bool completed) => _coreActualUseTargetProgramsOperations.CompleteActualUseTargetBinding(frame, completed);
    void global::CardGame.Core.IProgramWindowNativeReturns.CompleteForeignActualTurnStartBinding(ProgramSkillFrame frame) => _coreForeignTurnContestsOperations.CompleteForeignActualTurnStartBinding(frame);
    void global::CardGame.Core.IProgramWindowNativeReturns.CompleteDyingProgramBinding(ProgramSkillFrame frame, bool completed) => _coreProgramLifecycleOperations.CompleteDyingProgramBinding(frame, completed);
    void global::CardGame.Core.IProgramWindowNativeReturns.AdvanceDamageTriggerCandidate(DamageTriggerWindowFrame frame) => _coreNativeDamageWindowsOperations.AdvanceDamageTriggerCandidate(frame);
    void global::CardGame.Core.IProgramWindowNativeReturns.CompleteCardEffectCandidate(ProgramSkillWindowContext context) => _coreCardEffectProgramsOperations.CompleteCardEffectCandidate(context);
    void global::CardGame.Core.IProgramWindowNativeReturns.RestoreCompletedUseAttackAfterIssuedChild(ProgramSkillFrame frame, ProgramCardTriggerWindowFrame parent) => _coreCompletedUseChildReturnsOperations.RestoreCompletedUseAttackAfterIssuedChild(frame, parent);
    void global::CardGame.Core.IProgramWindowNativeReturns.CompleteProgramJudgmentReplacementBinding(ProgramSkillFrame frame, bool completed) => _coreJudgmentReplacementProgramsOperations.CompleteProgramJudgmentReplacementBinding(frame, completed);
}
