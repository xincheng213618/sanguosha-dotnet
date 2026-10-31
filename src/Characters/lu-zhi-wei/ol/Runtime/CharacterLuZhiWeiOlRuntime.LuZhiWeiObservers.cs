#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuZhiWeiOlRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 570)]
    internal bool AllowsSettleQingzhongActualPlayDebtTrigger(ProgramTriggerLegalityContext gate)
    {
        var features = gate.Features;
        var candidate = gate.Candidate;
        var context = gate.Context;
        return !(features.HasOperation(SkillProgramEffectOp.SettleQingzhongActualPlayDebt) && !HasIssuedQingzhongDebtCandidate(candidate, context));
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 350)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 350)]
    internal bool CanContinueIssuedLuZhiWei(ProgramSkillFrame f)
    {
        if (f.Qingzhong is null)
        {
            if (f.InstructionIndex != 0 || f.WindowContext is not { Window: SkillProgramTriggerWindow.PlayEnding, QingzhongDebt: { } issued } || f.TriggerId != issued.EndingBindingId || f.SkillId != issued.Source.SkillId || f.SkillInstanceId != issued.Source.SkillInstanceId || f.OwnerSeat != issued.Source.OwnerSeat || f.GameplayHash != issued.GameplayHash || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].QingzhongDebts.Contains(issued) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<QingzhongDebtArmedEvent>().Count(e => e.Debt == issued) != 1)
                return false;
            var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
            return i > 0 && _resolutionStackOperations._resolutionStack[i - 1] is ProgramLifecycleTriggerWindowFrame w && w.Window == SkillProgramTriggerWindow.PlayEnding && w.QingzhongEndingPhase == issued.Phase && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]) && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!.Triggers.Single(t => t.Id == f.TriggerId).Effects is [{ Op: SkillProgramEffectOp.SettleQingzhongActualPlayDebt }];
        }

        var p = f.Qingzhong!;
        var d = p.Debt;
        if (_corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != (p.Stage == QingzhongStage.DrawChildren ? SkillProgramEffectOp.DrawAndArmQingzhongActualPlayDebt : SkillProgramEffectOp.SettleQingzhongActualPlayDebt))
            return false;
        if (d.Source.OwnerSeat != f.OwnerSeat || d.Source.SkillId != f.SkillId || d.Source.SkillInstanceId != f.SkillInstanceId || d.GameplayHash != f.GameplayHash || p.InstructionIndex != f.InstructionIndex || f.InstructionIndex != 1 || d.Phase.OwnerSeat != f.OwnerSeat || !Enum.IsDefined(p.Stage) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<QingzhongDebtArmedEvent>().Count(e => e.Debt == d) != 1)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index < 1)
            return false;
        if (p.Stage == QingzhongStage.DrawChildren)
        {
            if (f.Id != d.ProgramFrameId || f.TriggerId != d.Source.BindingId || f.WindowContext?.QingzhongPhase != d.Phase || _resolutionStackOperations._resolutionStack[index - 1] is not PlayPhaseStartingBoundaryFrame w || w.Id != d.StartingWindowId || w.OwnerSeat != f.OwnerSeat || w.QingzhongStartingPhase != d.Phase || w.ItemIndex < 0 || w.ItemIndex >= w.Items.Count || w.Items[w.ItemIndex].Candidate is not { } candidate || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate))
                return false;
        }
        else
        {
            if (f.TriggerId != d.EndingBindingId || f.WindowContext?.QingzhongDebt != d || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramLifecycleTriggerWindowFrame w || w.QingzhongEndingPhase != d.Phase || w.Window != SkillProgramTriggerWindow.PlayEnding || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]))
                return false;
            if (p.Stage == QingzhongStage.ChooseMinimum)
                return f.PendingMovementContinuation is null && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].QingzhongDebts.Contains(d);
            if (p.TargetSeat is null || p.OwnerHandCount < 0 || p.TargetHandCount < 0 || p.ActualCount != (p.TargetSeat == f.OwnerSeat ? 0 : p.OwnerHandCount + p.TargetHandCount) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > p.Before && m.Sequence <= p.After && m.Reason.Value == QingzhongSwapReason && m.From == CardLocation.Hand(f.OwnerSeat) && m.To == CardLocation.Hand(p.TargetSeat.Value)) != (p.TargetSeat == f.OwnerSeat ? 0 : p.OwnerHandCount) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > p.Before && m.Sequence <= p.After && m.Reason.Value == QingzhongSwapReason && m.From == CardLocation.Hand(p.TargetSeat.Value) && m.To == CardLocation.Hand(f.OwnerSeat)) != (p.TargetSeat == f.OwnerSeat ? 0 : p.TargetHandCount) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<QingzhongDebtConsumedEvent>().Count(e => e.FrameId == f.Id && e.Debt == d && e.Settled) != 1)
                return false;
        }

        return p.Before >= 0 && p.After >= p.Before && p.After <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && p.ActualCount >= 0 && f.PendingMovementContinuation is { SubjectSeat: var seat, BeforeCount: 0, CoverageResultBind: null } && seat == f.OwnerSeat && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<QingzhongMovementIssuedEvent>().Count(e => e.FrameId == f.Id && e.Stage == p.Stage && e.Before == p.Before && e.After == p.After && e.ActualCount == p.ActualCount && e.TargetSeat == p.TargetSeat && e.OwnerHandCount == p.OwnerHandCount && e.TargetHandCount == p.TargetHandCount) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > p.Before && m.Sequence <= p.After && IsExactQingzhongMovement(f, p, m)) == p.ActualCount;
    }

    internal bool IsExactQingzhongMovement(ProgramSkillFrame f, QingzhongPending p, CardMovementRecord m) => p.Stage == QingzhongStage.DrawChildren ? m.Reason.Value == QingzhongDrawReason && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) : p.Stage == QingzhongStage.SwapChildren && p.TargetSeat is { } target && m.Reason.Value == QingzhongSwapReason && (m.From == CardLocation.Hand(f.OwnerSeat) && m.To == CardLocation.Hand(target) || m.From == CardLocation.Hand(target) && m.To == CardLocation.Hand(f.OwnerSeat));

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 350)]
    internal bool IsLuZhiWeiMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => (e?.Op is SkillProgramEffectOp.DrawAndArmQingzhongActualPlayDebt or SkillProgramEffectOp.SettleQingzhongActualPlayDebt) && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && CanContinueIssuedLuZhiWei(f);
    internal bool LuZhiWeiFirstChild(ProgramSkillFrame root, ResolutionFrame child) => CanContinueIssuedLuZhiWei(root) && root.Qingzhong is { } p && child is CardsMovedTriggerWindowFrame moved && moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > p.Before && m.Sequence <= p.After && IsExactQingzhongMovement(root, p, m));
    internal ProgramSkillFrame? LuZhiWeiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => _characterLuZhiWeiOlOperations.LuZhiWeiFirstChild(r, c), (i) => _coreObserverSubtreesOperations.HasMovementObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1530)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1640)]
    internal bool HasLuZhiWeiDamageObserver(long id) => LuZhiWeiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 520)]
    internal bool HasLuZhiWeiBeforeDamageObserver(long id) => LuZhiWeiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 240)]
    internal bool TryAdvanceLuZhiWeiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LuZhiWeiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
