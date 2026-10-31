#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDongZhaoOlRuntime
{
    internal ProgramSkillFrame? DongZhaoObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _characterDongZhaoOlOperations.DongZhaoFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasMovementObserverSuffix(index));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 430)]
    internal bool TryAdvanceDongZhaoSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && DongZhaoObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    internal bool ValidIssuedZaowang(ProgramSkillFrame f)
    {
        if (f.ZaowangPaidTargetSeat is not { } seat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || f.SelectedTargetSeats is not [var selected] || selected != seat || f.TriggerId is not null || f.InstructionIndex is < 1 or > 3)
            return false;
        var program = _contentRegistryOperations._contentRegistry!.GetSkill(f.SkillId).Program;
        var a = program is null ? null : ProgramInstructionResolver.Default.FindActivation(program, f.ActivationId);
        if (a?.Effects is not [{ Op: SkillProgramEffectOp.ZaowangResolve }, { Op: SkillProgramEffectOp.Recover, Amount: 1 }, { Op: SkillProgramEffectOp.Draw, Amount: 3 }])
            return false;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZaowangGrantedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        return facts is [var e] && e.OwnerSeat == f.OwnerSeat && e.TargetSeat == seat && e.SkillId == f.SkillId && e.SkillInstanceId == f.SkillInstanceId && e.GameplayHash == f.GameplayHash;
    }

    internal bool ZaowangFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!ValidIssuedZaowang(root) || root.InstructionIndex != 2 || root.ZaowangPaidTargetSeat is not { } target)
            return false;
        var grant = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZaowangGrantedEvent>().Single(e => e.FrameId == root.Id);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Continuation == PostEventContinuation.Program && hp.ResumeFrameId == root.Id && hp.Change.ParentFrameId == root.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == root.OwnerSeat && hp.Change.TargetSeat == target && hp.Change.Amount == 1 && hp.Change.HpBefore == grant.HpBeforeRecovery && hp.Change.HpAfter == grant.HpBeforeRecovery + 1 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RecoveryAppliedEvent>().Any(e => e.SourceSeat == root.OwnerSeat && e.TargetSeat == target && e.Amount == 1 && e.RemainingHp == hp.Change.HpAfter);
        if (child is RecoveryReplacementFrame recovery && _coreTypedRecoveryDamageCursorOperations.TypedRecoveryPausedEffect(root)is { } effect && effect.CompiledInstruction is RecoverProgramInstruction instruction)
            return recovery.Attempt.Amount == 1 && recovery.Attempt.TargetSeat == target && _coreTypedRecoveryDamageCursorOperations.DirectProgramRecoveryReturnMatches(root, effect, instruction, recovery);
        return false;
    }
}
