#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuaXiongMouMobileRuntime
{
    internal const string MouYaowu = MouHuaXiongComposition.Yaowu, MouYangwei = MouHuaXiongComposition.Yangwei;
    internal static CardConversionSource MouHuaXiongSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_hua_xiong_mou_mobile_MouHuaXiongMobile.MouHuaXiongSource(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29100)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29100)]
    internal bool KeepPaidMouHuaXiong(ProgramSkillFrame f)
    {
        if (f.MouHuaXiongYangwei is not null)
            return ValidMouYangweiReceipt(f);
        return f.MouHuaXiongYaowu is { Benefit: not null } && ValidMouYaowuReceipt(f);
    }

    internal bool MouHuaXiongPaidRoot(ProgramSkillFrame f) => f.MouHuaXiongYangwei is not null ? ValidMouYangweiReceipt(f) : f.MouHuaXiongYaowu is { Benefit: not null } && ValidMouYaowuReceipt(f);
    internal bool MouHuaXiongFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MouHuaXiongPaidRoot(f))
            return false;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, f))
            return true;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id;
        if (child is not CardsMovedTriggerWindowFrame moved || moved.Batch.ParentFrameId != f.Id || moved.ResumeProgramFrameId != f.Id && moved.Batch.AwaitingProgramFrameId != f.Id || moved.Batch.Movements.Count == 0)
            return false;
        var recipient = f.MouHuaXiongYangwei is not null ? f.OwnerSeat : f.MouHuaXiongYaowu!.Benefit == "source-draw" ? f.MouHuaXiongYaowu.SourceSeat : f.OwnerSeat;
        return moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(recipient!.Value) && m.Reason.Value == $"skill-program.{(f.SkillId)}.{(SkillProgramEffectOp.Draw)}");
    }

    internal bool MouHuaXiongObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => _coreObserverSubtreesOperations.IsMovementObserverEdge(child) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(child) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(child) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[child - 1], _resolutionStackOperations2._resolutionStack()[child]) || _resolutionStackOperations2._resolutionStack()[child - 1] is ProgramSkillFrame f && _characterHuaXiongMouMobileOperations.MouHuaXiongFirstChild(f, _resolutionStackOperations2._resolutionStack()[child]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying));
    internal ProgramSkillFrame? MouHuaXiongObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterHuaXiongMouMobileOperations.MouHuaXiongFirstChild(f, child), (index) => _characterHuaXiongMouMobileOperations.MouHuaXiongObserverSuffix(index));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1270)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1380)]
    internal bool HasMouHuaXiongDamageObserver(long id) => MouHuaXiongObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1410)]
    internal bool HasMouHuaXiongBeforeDamageObserver(long id) => MouHuaXiongObserverRoot()is { } root && (root.MouHuaXiongYaowu?.WindowId == id || _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 800)]
    internal bool IsMouHuaXiongDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MouHuaXiongObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 570)]
    internal bool IsMouHuaXiongRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (!MouHuaXiongPaidRoot(f) || recovery.ParentFrameId == f.Id)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && MouHuaXiongFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && MouHuaXiongObserverSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 29100)]
    internal bool AdvanceMouHuaXiongChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouHuaXiongObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    SkillProgramStepOutcome global::CardGame.Core.IMouHuaXiongHost.Yangwei(ProgramSkillFrame f) => ResolveMouYangwei(f);
    SkillProgramStepOutcome global::CardGame.Core.IMouHuaXiongHost.Yaowu(ProgramSkillFrame f) => ResolveMouYaowu(f);
}
