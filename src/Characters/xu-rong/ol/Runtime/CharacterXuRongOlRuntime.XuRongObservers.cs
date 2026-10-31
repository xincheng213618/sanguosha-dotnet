#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXuRongOlRuntime
{
    internal void CaptureXuRongActualDamageHp(long damageFrameId, global::CardGame.Core.GameEngine.IDamageAttempt attack, int hpBefore, int actualAmount)
    {
        if (!_coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(p).GetInstanceTriggers(SkillProgramTriggerWindow.DyingEntering).Any(b => b.Trigger.Effects is [{ Op: SkillProgramEffectOp.ReplenishShaJueAndClaimDyingCause }])))
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame d || d.Id != damageFrameId || d.ParentFrameId != attack.ResolutionId || d.TargetSeat != attack.TargetSeat || d.Amount != actualAmount || actualAmount <= 0 || _coreMatchParticipantsPlayersAccess._players[d.TargetSeat].Hp != hpBefore || !_coreMatchParticipantsPlayersAccess._players[d.TargetSeat].HasHpValue || d.XuRongActualHp is not null)
            throw new InvalidOperationException("Xu Rong requires the exact native pre-clamp damage payment.");
        if (d.ArmorPayment is { } armor && (armor.HpBefore != hpBefore || !_coreDamageArmorPaymentsOperations.IsValidDamageArmorPayment(d, requireApplied: false)))
            throw new InvalidOperationException("Xu Rong lost the exact Armor-adjusted native damage payment.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(d.Id, d with { XuRongActualHp = new(d.Id, attack.ResolutionId, d.TargetSeat, hpBefore, actualAmount, checked(hpBefore - (d.ArmorPayment?.HpLossAmount ?? actualAmount))) });
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 460)]
    internal void ObserveXuRongDyingQualification(IGameEvent payload)
    {
        if (payload is not PlayerDyingEvent fact || _coreDyingCursorRuntimeOperations.ActiveDying is not { } dying || dying.Id != fact.ResolutionId || dying.VictimSeat != fact.VictimSeat || dying.XuRongProof is not null)
            return;
        if (!_coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(p).GetInstanceTriggers(SkillProgramTriggerWindow.DyingEntering).Any(b => b.Trigger.Effects is [{ Op: SkillProgramEffectOp.ReplenishShaJueAndClaimDyingCause }])))
            return;
        var victim = _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat];
        if (!victim.HasHpValue || victim.Hp > 0)
            return;
        var attack = _coreNativeDamageHandlesOperations.GetDyingAttack(dying);
        var originalHp = victim.Hp;
        long? actionId = null;
        var ids = Array.Empty<int>();
        if (dying.Continuation == DyingContinuationKind.Damage && attack is not null && attack.TargetSeat == victim.Seat && attack.DamageWasApplied && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == dying.ParentFrameId)is DamageFrame damage && damage.ParentFrameId == attack.ResolutionId && damage.TargetSeat == victim.Seat)
        {
            if (damage.XuRongActualHp is not { } paid || paid.DamageFrameId != damage.Id || paid.DamageAttemptResolutionId != attack.ResolutionId || paid.TargetSeat != victim.Seat || paid.ActualAmount != damage.Amount || paid.ActualAmount != attack.DamageAmount || paid.UnclampedHpAfter != paid.HpBefore - (damage.ArmorPayment?.HpLossAmount ?? paid.ActualAmount) || damage.ArmorPayment is { } armor && (armor.HpBefore != paid.HpBefore || !_coreDamageArmorPaymentsOperations.IsIssuedDamageArmorPayment(damage)) || (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageAppliedEvent>().LastOrDefault()is not { } applied || applied.TargetSeat != victim.Seat || applied.Amount != paid.ActualAmount || applied.RemainingHp != paid.UnclampedHpAfter || applied.SourceLess != attack.IsSourceLess || applied.SourceSeat != attack.SourceSeat || applied.Nature != damage.Nature || applied.ArmorPayment != damage.ArmorPayment))
                throw new InvalidOperationException("Xu Rong dying requirement lost its exact native applied damage proof.");
            originalHp = paid.UnclampedHpAfter;
            var native = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId);
            if (native is { Action: { Type: CardActionType.Use } action } && native.CardKind == attack.EffectiveCardKind && (native.PhysicalCardIds ?? (native.CardId > 0 ? new[]
            {
                native.CardId
            }

            : Array.Empty<int>()))is { } physical && physical.SequenceEqual(action.PhysicalCards.Select(c => c.CardId)) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == action.ActionId && e.Action.PhysicalCards.Select(c => c.CardId).SequenceEqual(physical)))
            {
                ids = physical.ToArray();
                actionId = action.ActionId;
            }
            else if (attack.IsDelayedJudgmentDamage && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == attack.ResolutionId)is JudgmentFrame { Continuation: JudgmentContinuationKind.Lightning, DelayedCardId: { } held, Succeeded: true } judgment && attack.EffectiveCardKind == CardKind.Lightning && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<LightningResolvedEvent>().Any(e => e.ResolutionId == judgment.Id && e.CardId == held && e.Hit))
                ids = [held];
        }

        var entities = ids.Distinct().Where(id => id > 0).Select(id => new XuRongCausalEntity(id, _cardZonesOperations._cardZones.GetLocation(id))).Where(e => e.OriginalLocation.Zone is CardZoneKind.Processing or CardZoneKind.Judgment or CardZoneKind.DiscardPile).ToArray();
        var proof = new XuRongDyingProof(dying.Id, dying.ParentFrameId, dying.VictimSeat, originalHp, checked(1 - originalHp), attack?.ResolutionId, actionId, attack is null || attack.IsSourceLess || attack.IsDelayedJudgmentDamage ? null : attack.SourceSeat, attack is null || attack.IsSourceLess || attack.IsDelayedJudgmentDamage, Array.AsReadOnly(entities));
        _coreNativeDyingFramesOperations.UpdateDyingFrame(dying.Id, current => current with { XuRongProof = proof });
    }

    internal ProgramSkillWindowContext CreateXuRongDyingContext(ProgramSkillWindowContext context) => context.Window == SkillProgramTriggerWindow.DyingEntering && _coreDyingCursorRuntimeOperations.ActiveDying?.XuRongProof is { } proof ? context with
    {
        XuRongDying = proof
    }

    : context;
    internal bool HasXuRongDyingQualification(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects is [{ Op: SkillProgramEffectOp.ReplenishShaJueAndClaimDyingCause }] && context.Window == SkillProgramTriggerWindow.DyingEntering && context.XuRongDying is { } proof && proof.VictimSeat != candidate.OwnerSeat && context.TargetSeat == proof.VictimSeat && proof.HpAtEntry <= 0 && proof.RescueCardsRequired == 1 - proof.HpAtEntry && _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().Any(d => d.Id == proof.DyingFrameId && d.XuRongProof == proof);
    internal bool ExactXuRongParent(ProgramSkillFrame f, XuRongPending r)
    {
        if (f.Id <= 0 || f.GameplayHash != r.GameplayHash || f.InstructionIndex != r.InstructionIndex || f.InstructionIndex != 1 || f.WindowContext?.ParentFrameId != r.ParentFrameId || XuRongSource(f) != r.Source || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.GameplayHash)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (i < 1)
            return false;
        if (r.DyingFrameId is { } dying)
            return f.WindowContext?.Window == SkillProgramTriggerWindow.DyingEntering && f.WindowContext.XuRongDying is { } proof && proof.DyingFrameId == dying && proof.VictimSeat == r.SubjectSeat && _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().SingleOrDefault(d => d.Id == dying)?.XuRongProof == proof && _resolutionStackOperations._resolutionStack[i - 1] is ProgramLifecycleTriggerWindowFrame w && w.ResumeDyingFrameId == dying && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]);
        return f.WindowContext?.Window == SkillProgramTriggerWindow.PlayPhaseStarting && _resolutionStackOperations._resolutionStack[i - 1] is PlayPhaseStartingBoundaryFrame phase && phase.Id == r.ParentFrameId && phase.OwnerSeat == r.SubjectSeat && phase.ItemIndex >= 0 && phase.ItemIndex < phase.Items.Count && phase.Items[phase.ItemIndex].Candidate is { } c && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c);
    }

    internal bool ValidXuRong(ProgramSkillFrame f)
    {
        if (f.XuRong is not { } r || !ExactXuRongParent(f, r))
            return false;
        if (r.DyingFrameId is { } dying)
        {
            if (_corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ReplenishShaJueAndClaimDyingCause || f.WindowContext?.XuRongDying is not { } proof || proof.DyingFrameId != dying || proof.RescueCardsRequired != 1 - proof.HpAtEntry || proof.HpAtEntry > 0 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XionghuoPoolChangedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.Kind == "dying") != 1)
                return false;
        }
        else if (_corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.SettleXionghuoMarkedPlay || r.Branch is < 0 or > 2 || r.RandomBefore == r.RandomAfter || r.Grants.Count == 0 || r.Grants.Any(g => g.TargetSeat != r.SubjectSeat || g.Source.OwnerSeat != f.OwnerSeat || g.Count < 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XionghuoMarkTransferredEvent>().Count(e => e.Grant == g) != 1) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XionghuoPlayDebtPaidEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.PlayWindowId == r.ParentFrameId && e.SubjectSeat == r.SubjectSeat && e.ActualTurnNumber == r.ActualTurnNumber && e.PhaseInstanceId == r.PhaseInstanceId && e.Branch == r.Branch && e.RandomBefore == r.RandomBefore && e.RandomAfter == r.RandomAfter && e.RemovedCount == r.Grants.Sum(g => g.Count)) != 1)
            return false;
        if (r.Stage is "fire-native" or "hp-native")
            return f.PendingMovementContinuation is null;
        if (r.Stage is not ("take-native" or "claim-native") || r.MovementBefore < 0 || r.MovementAfter < r.MovementBefore || r.MovementAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || f.PendingMovementContinuation is not { SubjectSeat: var seat, BeforeCount: 0, CoverageResultBind: null } || seat != f.OwnerSeat)
            return false;
        var reason = r.Stage == "claim-native" ? XuRongClaimReason : XuRongTakeReason;
        return r.PaidCardIds.Distinct().Count() == r.PaidCardIds.Count && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<XuRongCardsPaidEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.Kind == reason && e.Before == r.MovementBefore && e.After == r.MovementAfter && e.CardIds.SequenceEqual(r.PaidCardIds)) == 1 && r.PaidCardIds.All(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == id && m.Sequence > r.MovementBefore && m.Sequence <= r.MovementAfter && m.Reason.Value == reason && m.To == CardLocation.Hand(f.OwnerSeat)) == 1);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 190)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 190)]
    internal bool CanContinueIssuedXuRong(ProgramSkillFrame f) => ValidXuRong(f);
    internal bool XuRongFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!ValidXuRong(root) || root.XuRong is not { } r)
            return false;
        if (r.Stage == "fire-native" && root.AttackAttempt is { SourceLess: false, TargetSeat: var target, SourceSeat: var source } attack && target == r.SubjectSeat && source == root.OwnerSeat)
        {
            if (child is DamageFrame damage)
                return damage.ParentFrameId == root.Id && damage.TargetSeat == target && damage.SourceSeat == source && damage.Amount == attack.DamageAmount && damage.Nature == attack.Nature;
            if (child is BeforeDamageProgramWindowFrame before)
                return (before.ContinuationAttackResolutionId ?? before.ParentFrameId) == root.Id && before.TargetSeat == target && before.SourceSeat == source && before.Amount == attack.DamageAmount && before.Nature == attack.Nature;
        }

        if (child is HpChangedTriggerWindowFrame hp && r.Stage == "hp-native")
            return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.TargetSeat == r.SubjectSeat && hp.Change.Amount == 1 && hp.Change.Kind == HpChangeKind.Loss && hp.Change.SourceSeat is null;
        if (child is DyingFrame dying && r.Stage == "hp-native")
            return dying.ParentFrameId == root.Id && dying.VictimSeat == r.SubjectSeat && dying.Continuation == DyingContinuationKind.ProgramSkill;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => m.Sequence > r.MovementBefore && m.Sequence <= r.MovementAfter && r.PaidCardIds.Contains(m.CardId) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        return false;
    }

    internal ProgramSkillFrame? XuRongObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => this.XuRongFirstChild(r, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 260)]
    internal bool TryAdvanceXuRongSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && XuRongObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    [ProgramContinuation(ProgramContinuationKind.Resume, 23600)]
    internal bool ResumeXuRong(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { XuRong: { } r } f || f.Id != id)
            return false;
        if (f.AttackAttempt is not null)
            return true;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, f.PendingMovementContinuation is null ? PostEventContinuation.Program : PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XuRongCompletedEvent(f.Id, r.Source, r.Stage, r.SubjectSeat, r.DyingFrameId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XuRong = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 23600)]
    internal bool ReturnXuRongMovement(ProgramSkillFrame f) => f.XuRong is not null && ResumeXuRong(f.Id);
}
