#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineForcedPhaseAlcoholRuntime
{
    internal bool HasPhaseAlcoholChain(ProgramSkillFrame frame) => _contentRegistryOperations._contentRegistry.ProgramDependencies.GetTriggerOperationSkillIds(SkillProgramEffectOp.UsePhaseOwnerVirtualAlcohol).Contains(frame.SkillId) && ForcedPhaseAlcoholContract.IsChain(ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).Instructions);
    internal bool CanUsePhaseOwnerAlcohol(ProgramSkillWindowContext? context) => context is { Window: SkillProgramTriggerWindow.PlayPhaseStarting, SourceSeat: { } seat, TargetSeat: { } target } && seat == target && seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreVirtualBasicOffersOperations.VirtualBasicOptions2(seat).Contains((CardKind.Alcohol, seat)) && _resolutionStackOperations._resolutionStack.OfType<PlayPhaseStartingBoundaryFrame>().Any(p => p.Id == context.ParentFrameId && p.OwnerSeat == seat);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 90)]
    internal bool CanOfferPhaseAlcohol(SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.UsePhaseOwnerVirtualAlcohol) || CanUsePhaseOwnerAlcohol(context);
    internal bool PreparePhaseAlcoholPayment(ProgramSkillFrame f, string bind, CardMoveReason reason)
    {
        if (!HasPhaseAlcoholChain(f))
            return true;
        if (f.PhaseAlcohol is not null || f.InstructionIndex != 2 || !CanUsePhaseOwnerAlcohol(f.WindowContext))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "该出牌阶段角色当前不能使用酒，未支付弃牌费用。");
            return false;
        }

        var cost = _coreProgramLifecycleOperations.GetProgramCardSet(f, bind);
        if (cost.CardIds is not [var id] || cost.SourceLocations is not [var from] || from != CardLocation.Hand(f.OwnerSeat) || _cardZonesOperations._cardZones.GetLocation(id) != from || cost.SelectionActorSeat is { } selector && selector != f.OwnerSeat)
            throw new InvalidOperationException("Phase Wine requires its one exact owner hand cost.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PhaseAlcohol = new(2, 3, f.WindowContext!.ParentFrameId, f.WindowContext.SourceSeat!.Value, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, id, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, 0, reason.Value, ProgramPhaseAlcoholStage.Paid) });
        return true;
    }

    internal void CommitPhaseAlcoholPayment(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (f.PhaseAlcohol is not { Stage: ProgramPhaseAlcoholStage.Paid, PaymentSequenceAfter: 0 } receipt)
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PhaseAlcohol = receipt with { PaymentSequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        if (!HasExactPhaseAlcoholReceipt(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)))
            throw new InvalidOperationException("Phase Wine did not commit its one exact hand-to-discard payment.");
    }

    internal bool HasExactPhaseAlcoholReceipt(ProgramSkillFrame f)
    {
        if (f.PhaseAlcohol is not { } r || f.WindowContext is not { Window: SkillProgramTriggerWindow.PlayPhaseStarting } context || !HasPhaseAlcoholChain(f) || r.PaymentInstruction != 2 || r.WineInstruction != 3 || f.InstructionIndex is < 2 or > 3 || context.ParentFrameId != r.PhaseParentFrameId || context.SourceSeat != r.ActorSeat || context.TargetSeat != r.ActorSeat || context.OwnerSeat != f.OwnerSeat || r.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || r.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || r.PaymentSequenceAfter <= r.PaymentSequenceBefore || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || _resolutionStackOperations._resolutionStack.OfType<PlayPhaseStartingBoundaryFrame>().SingleOrDefault(p => p.Id == r.PhaseParentFrameId)is not { } parent || parent.ItemIndex >= parent.Items.Count || parent.Items[parent.ItemIndex].Candidate is not { } candidate || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate) || candidate.BindingId != f.TriggerId || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == r.CardId && m.Sequence > r.PaymentSequenceBefore && m.Sequence <= r.PaymentSequenceAfter && m.From == CardLocation.Hand(f.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason.Value == r.PaymentReason) != 1)
            return false;
        return r.Stage == ProgramPhaseAlcoholStage.Paid ? r.WineFrameId is null && !r.WineSkipped : r.Stage == ProgramPhaseAlcoholStage.WineIssued ? r.WineFrameId is not null && !r.WineSkipped : r.Stage == ProgramPhaseAlcoholStage.DamageIssued && (r.WineFrameId is not null || r.WineSkipped);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 490)]
    internal bool CanContinuePhaseAlcohol(ProgramSkillFrame frame) => frame.PhaseAlcohol is { } r && _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.ActorSeat].IsAlive && HasExactPhaseAlcoholReceipt(frame);
    internal SkillProgramStepOutcome BeginPhaseOwnerAlcohol(ProgramSkillFrame frame, int damageToOther)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (f.PhaseAlcohol is not { Stage: ProgramPhaseAlcoholStage.Paid } receipt || f.InstructionIndex != 3 || f.PendingMovementContinuation is not null || !HasExactPhaseAlcoholReceipt(f))
            throw new InvalidOperationException("Phase Wine requires its once-paid, drained actual phase instruction.");
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[receipt.ActorSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        // A later payment child can change Wine legality. Its independent damage
        // tail is still the paid skill effect, just as a canceled Wine is.
        if (!CanUsePhaseOwnerAlcohol(f.WindowContext))
        {
            if (receipt.ActorSeat != f.OwnerSeat && damageToOther > 0)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PhaseAlcohol = receipt with { Stage = ProgramPhaseAlcoholStage.DamageIssued, WineSkipped = true } });
                return _coreSkillProgramsOperations.BeginProgramSkillDamage(f, receipt.ActorSeat, damageToOther);
            }

            _coreSkillProgramsOperations.FinishProgramSkill(f, completed: true);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var actor = _coreMatchParticipantsPlayersAccess._players[receipt.ActorSeat];
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PhaseAlcohol = receipt with { Stage = ProgramPhaseAlcoholStage.WineIssued, WineFrameId = id }, VirtualBasicDraft = new(f.InstructionIndex, id) });
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, null, CardActionType.Use, actor.Seat, f.OwnerSeat, null, null, null, CardKind.Alcohol, [actor.Seat], [], [], effectiveSuit: Suit.None, effectiveRank: 0));
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(id, actor.Seat, 0, CardKind.Alcohol, [actor.Seat], PhysicalCardIds: []) { Action = action, VirtualBasicReturn = new(f.Id, f.InstructionIndex), FirstOwnPlayUseDistanceUnlimited = _coreIssuedPlayUsePoliciesOperations.HasFirstActualPlayUseDistance(actor) });
        // The use allowance is spent by the real actor at commitment, even when
        // a card-use trigger later cancels Wine's effect.
        _characterHuangGaiMouMobileOperations.RecordMouZhaJiangAwareAlcoholDebit(id, actor);
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(id, 0, CardKind.Alcohol, actor.Seat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(id, [actor.Seat]));
        _characterLuMengMouOperations.RecordYingboCardUse(id, actor.Seat, CardKind.Alcohol);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor.Seat, CardKind.Alcohol);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(0, SimpleCardUseEffect.Alcohol));
        return SkillProgramStepOutcome.AwaitChild;
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 645)]
    internal bool ResumePhaseAlcohol(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { PhaseAlcohol: { } r } f || f.Id != id)
            return false;
        if (!HasExactPhaseAlcoholReceipt(f))
            throw new InvalidOperationException("Phase Wine lost its exact paid parent or actual phase.");
        if (r.Stage == ProgramPhaseAlcoholStage.Paid)
            return false;
        if (f.VirtualBasicDraft is not null)
            throw new InvalidOperationException("Phase Wine resumed before its typed zero-entity child returned.");
        if (r.Stage == ProgramPhaseAlcoholStage.DamageIssued)
        {
            if (f.AttackAttempt is not null)
            {
                _coreNativeDamageCompletionOperations.CompleteDamageAttack(_kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(id));
                return true;
            }

            _coreSkillProgramsOperations.FinishProgramSkill(f, completed: true);
            return true;
        }

        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseFinishedEvent>().Count(e => e.ResolutionId == r.WineFrameId && e.CardId == 0 && e.CardKind == CardKind.Alcohol) != 1)
            throw new InvalidOperationException("Phase Wine needs one exact completed declaration, including a canceled effect.");
        var damage = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions[2].Amount;
        if (r.ActorSeat != f.OwnerSeat && damage > 0 && CanContinuePhaseAlcohol(f))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PhaseAlcohol = r with { Stage = ProgramPhaseAlcoholStage.DamageIssued } });
            _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.ActorSeat, damage);
            return true;
        }

        _coreSkillProgramsOperations.FinishProgramSkill(f, completed: true);
        return true;
    }

    internal (int Deaths, int Kills) ActualTurnDeathCounts(int ownerSeat)
    {
        var deaths = 0;
        var kills = 0;
        var started = false;
        foreach (var item in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory())
        {
            if (item is TurnStartedEvent begun)
            {
                started = begun.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && begun.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat;
                if (started)
                {
                    deaths = 0;
                    kills = 0;
                }

                continue;
            }

            if (!started)
                continue;
            if (item is TurnEndedEvent ended && ended.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && ended.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat)
                break;
            if (item is PlayerDiedEvent death)
            {
                deaths++;
                if (death.KillerSeat == ownerSeat)
                    kills++;
            }
        }

        return (deaths, kills);
    }

    internal int ActualTurnDeathDrawCount(ProgramSkillFrame f, int seat)
    {
        if (seat != f.OwnerSeat || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterTurnEnded, Facts: { ActualTurnDeathCount: { } deaths, OwnerActualTurnKillCount: { } kills } } context || _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId)is not { } parent || !_coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(parent) || parent.AfterTurnEnded?.CurrentChild is not { } child || child.FrameId != f.Id || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, child.Candidate) || !_coreAfterTurnEndedProgramsOperations.CanRunAfterTurnEndedCandidate(child.Candidate, context, _coreProgramLifecycleOperations.GetProgramTrigger2(child.Candidate)) || context.Facts != parent.AfterTurnEnded.Items[parent.AfterTurnEnded.ItemIndex].Facts || ActualTurnDeathCounts(f.OwnerSeat) != (deaths, kills) || deaths <= 0 || kills < 0 || kills > deaths)
            throw new InvalidOperationException("Death benefit draw requires exact frozen deaths from its completed actual turn.");
        var effect = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
        return checked(effect.ActualTurnDeathOwnerKillsOnly == true ? 3 * kills : deaths + 2 * kills);
    }

    SkillProgramStepOutcome global::CardGame.Core.IForcedPhaseAlcoholHost.UsePhaseOwnerVirtualAlcohol(ProgramSkillFrame f, int damageToOther) => BeginPhaseOwnerAlcohol(f, damageToOther);
}
