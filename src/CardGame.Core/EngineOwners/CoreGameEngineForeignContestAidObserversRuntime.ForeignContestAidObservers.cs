#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineForeignContestAidObserversRuntime
{
    internal bool SameTypeAidParentMatches(ProgramSkillFrame f)
    {
        if (f.SameTypeAid is not { } r || r.InstructionIndex != f.InstructionIndex || f.InstructionIndex != 1 || r.Source != _coreSameTypeActualUseAidOperations.SameTypeAidSource(f) || r.GameplayHash != f.GameplayHash || r.Use.OriginalOwnerTarget != f.OwnerSeat || !_coreSameTypeActualUseAidOperations.MatchesSameTypeAidUse(r.Use) || f.WindowContext is not { } context || _coreSameTypeActualUseAidOperations.FreezeSameTypeAidUse(f.OwnerSeat, context) != r.Use || !global::CardGame.Core.EngineOwners.CharacterFuHuangHouClassicRuntime.MatchesClassicQiuyuanAidContext(f.SkillId, f.OwnerSeat, context, r.Use))
            return false;
        var parent = _resolutionStackOperations._resolutionStack.SingleOrDefault(p => p.Id == context.ParentFrameId);
        if (parent is ProgramCardTriggerWindowFrame card)
            return card.ParentFrameId == r.Use.CardUseFrameId && card.CandidateIndex >= 0 && card.CandidateIndex < card.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(card.Candidates[card.CandidateIndex]));
        return parent is ActualUseTargetWindowFrame legacy && legacy.ParentFrameId == r.Use.CardUseFrameId && legacy.CandidateIndex >= 0 && legacy.CandidateIndex < legacy.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, legacy.Candidates[legacy.CandidateIndex]) && legacy.Contexts[legacy.CandidateIndex].ActualUseTarget == context.ActualUseTarget;
    }

    internal bool IsValidSameTypeAidPayment(ProgramSkillFrame f)
    {
        if (f.SameTypeAid is not { RecipientSeat: { } donor, Payment: { } paid } r || paid.From.OwnerSeat != donor || !global::CardGame.Core.EngineOwners.CharacterFuHuangHouClassicRuntime.SameTypeAidPaymentZones(f.SkillId).Contains(paid.From.Zone) || !paid.Delivered || paid.CardId <= 0 || paid.MovementSequence <= 0 || paid.SequenceAfter < paid.MovementSequence || !global::CardGame.Core.EngineOwners.CharacterFuHuangHouClassicRuntime.IsSameTypeAidPaymentKind(f.SkillId, paid.PrintedKind, r.Use.EffectiveKind) || _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence == paid.MovementSequence && m.CardId == paid.CardId && m.CardKind == paid.PrintedKind && m.From == paid.From && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == "program.same-type-actual-use-aid.gift") != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SameTypeAidGiftPaidEvent>().Count(e => e.ProgramFrameId == f.Id && e.Source == r.Source && e.Use == r.Use && e.RecipientSeat == donor && e.MovementSequence == paid.MovementSequence && e.SequenceAfter == paid.SequenceAfter && e.Delivered == paid.Delivered) != 1)
            return false;
        return !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > paid.MovementSequence && m.Sequence <= paid.SequenceAfter && !(paid.PrintedKind == CardKind.WoodenOx && paid.From == CardLocation.Equipment(donor) && m.From == CardLocation.Grain(donor) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard));
    }

    internal void AssertSameTypeAid(ProgramSkillFrame f)
    {
        if (f.SameTypeAid is not { } r)
            return;
        if (!SameTypeAidParentMatches(f) || !Enum.IsDefined(r.Stage) || r.RecipientSeat is { } donor && (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(donor) || donor == f.OwnerSeat || donor == r.Use.ActorSeat) || r.Stage == SameTypeAidStage.ChoosingRecipient && (r.RecipientSeat is not null || r.Payment is not null || r.AddedTarget) || r.Stage != SameTypeAidStage.ChoosingRecipient && r.RecipientSeat is null || r.Payment is not null && (!IsValidSameTypeAidPayment(f) || r.AddedTarget) || r.Stage == SameTypeAidStage.GiftChildren && r.Payment is null || r.AddedTarget && (r.Stage != SameTypeAidStage.Complete || r.RecipientSeat is not { } extra || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SameTypeAidTargetAddedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Source == r.Source && e.Use == r.Use && e.RecipientSeat == extra) != 1))
            throw new InvalidOperationException("Aid changed its exact actual use, recipient or committed payment.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 10)]
    internal bool IsForeignContestOrAidMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => pending.BeforeCount == 0 && pending.CoverageResultBind is null && (effect?.Op == SkillProgramEffectOp.ResolveForeignTurnPindian && f.ForeignTurnContest is { Stage: ForeignTurnContestStage.ClaimChildren } && pending.SubjectSeat == f.OwnerSeat || effect?.Op == SkillProgramEffectOp.OfferSameTypeDifferentNameOrExtraTarget && f.SameTypeAid is { Stage: SameTypeAidStage.GiftChildren, RecipientSeat: { } donor } && pending.SubjectSeat == donor);
    internal ProgramSkillFrame? ForeignContestAidPaidObserverRoot(long? exactDamageWindow = null)
    {
        for (var rootIndex = 1; rootIndex < _resolutionStackOperations._resolutionStack.Count; rootIndex++)
        {
            if (_resolutionStackOperations._resolutionStack[rootIndex] is not ProgramSkillFrame root)
                continue;
            int first, after, recoverySeat;
            int? paidId;
            CardLocation paidFrom;
            string reason;
            if (root.ForeignTurnContest is { Stage: ForeignTurnContestStage.ClaimChildren, ClaimedCardId: { } claimId, MovementSequence: { } sequence } contest && _foreignTurnContestOperations.Get().MatchesOrigin(root, contest.SourceBind, contest.Origin))
            {
                first = after = sequence;
                recoverySeat = root.OwnerSeat;
                paidId = claimId;
                paidFrom = CardLocation.DiscardPile;
                reason = ForeignTurnContestRuntime.ClaimReason;
            }
            else if (root.SameTypeAid is { Stage: SameTypeAidStage.GiftChildren, RecipientSeat: { } donor, Payment: { } payment } && SameTypeAidParentMatches(root) && IsValidSameTypeAidPayment(root))
            {
                first = payment.MovementSequence;
                after = payment.SequenceAfter;
                recoverySeat = donor;
                paidId = payment.CardId;
                paidFrom = payment.From;
                reason = "program.same-type-actual-use-aid.gift";
            }
            else
                continue;
            if (exactDamageWindow is { } windowId && !_resolutionStackOperations._resolutionStack.Skip(rootIndex + 1).Any(f => f.Id == windowId && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame))
                continue;
            if (rootIndex == _resolutionStackOperations._resolutionStack.Count - 1)
                return root;
            if (root.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null })
                continue;
            if (_resolutionStackOperations._resolutionStack[rootIndex + 1] is CardsMovedTriggerWindowFrame moved)
            {
                if (moved.Batch.ParentFrameId != root.Id || moved.Batch.OriginSkillId != root.SkillId || moved.Batch.OriginSkillInstanceId != root.SkillInstanceId || moved.Batch.OriginOwnerSeat != root.OwnerSeat || moved.Batch.AwaitingProgramFrameId is { } waiting && waiting != root.Id || moved.Batch.Movements.Count == 0 || moved.Batch.Movements.Any(m => m.Sequence < first || m.Sequence > after || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m)))
                    continue;
            }
            else
            {
                // An actual paid SilverLion removal can precede movement observers.
                if (paidFrom != CardLocation.Equipment(recoverySeat) || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence == first && m.CardId == paidId && m.CardKind == CardKind.SilverLion && m.From == paidFrom && m.To == CardLocation.Hand(root.OwnerSeat) && m.Reason.Value == reason))
                    continue;
                if (_resolutionStackOperations._resolutionStack[rootIndex + 1] is HpChangedTriggerWindowFrame hp)
                {
                    if (hp.Change.ParentFrameId != root.Id || hp.ResumeFrameId != root.Id || hp.Continuation != PostEventContinuation.AwaitedProgramMovement || hp.Change.Kind != HpChangeKind.Recovery || hp.Change.SourceSeat != recoverySeat || hp.Change.TargetSeat != recoverySeat || hp.Change.Amount != 1)
                        continue;
                }
                else if (_resolutionStackOperations._resolutionStack[rootIndex + 1] is RecoveryReplacementFrame recovery)
                {
                    if (!_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root) || recovery.Return.Continuation != PostEventContinuation.AwaitedProgramMovement || recovery.Attempt.SourceSeat != recoverySeat || recovery.Attempt.TargetSeat != recoverySeat || recovery.Attempt.Amount != 1 || recovery.Attempt.Completion.Producer != RecoveryAttemptProducer.SilverLion || recovery.Attempt.Completion.MoveReason?.Value != reason)
                        continue;
                }
                else
                    continue;
            }

            var valid = true;
            for (var i = rootIndex + 1; i < _resolutionStackOperations._resolutionStack.Count; i++)
            {
                // Root and first ledger edge have already been proved. No global
                // movement/Dying helper is widened by this local union.
                if (!_corePaidColorDamageClaimObserversOperations.PaidColorDamageClaimObserverEdge(i))
                {
                    valid = false;
                    break;
                }

                if (_resolutionStackOperations._resolutionStack[i] is DyingFrame d && (_coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(i, d) || _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(i, d) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(i, d) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(i, d)))
                    break;
            }

            if (valid)
                return root;
        }

        return null;
    }

    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1800)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 180)]
    internal bool HasForeignContestAidDamageObserver(long window) => ForeignContestAidPaidObserverRoot(window)is not null;
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 620)]
    internal bool IsForeignContestAidProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && ForeignContestAidPaidObserverRoot(_coreDamageCursorRuntimeOperations.ActiveDamageTrigger?.Id)is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);

    internal bool TryGetSameTypeAidVirtualPrimaryReturn(CardUseFrame use, ProgramSkillFrame parent, out IReadOnlyList<int> primary)
    {
        primary = [];
        if (!_coreSameTypeActualUseAidOperations.HasSameTypeAidTargetTail(use) || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.CardKind != CardKind.Slash || use.CardAttack is not { CardId: null, EffectiveCardKind: CardKind.Slash, PhysicalCardIds.Count: 0 } attack || attack.ProgramSkillCardUseFrameId != parent.Id || attack.SourceSeat != use.SourceSeat || use.Action is not { Type: CardActionType.Use, EffectiveKind: CardKind.Slash } action || action.ActorSeat != use.SourceSeat || action.ProviderSeat != use.SourceSeat || action.PhysicalCards.Count != 0 || action.RequesterSeat is not null || action.ResponderSeat is not null || action.OpponentSeat is not null || action.EffectiveSuit != Suit.None || action.EffectiveRank != 0 || parent.InstructionIndex < 1 || !action.TargetSeats.SequenceEqual(use.TargetSeats))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        if (index <= 0 || _resolutionStackOperations._resolutionStack[index - 1].Id != parent.Id || _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program is not { } program || program.GameplayHash != parent.GameplayHash)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (history.OfType<TargetsConfirmedEvent>().SingleOrDefault(e => e.ResolutionId == use.Id)is not { TargetSeats.Count: 1 } declared || history.OfType<CardUseDeclaredEvent>().SingleOrDefault(e => e.ResolutionId == use.Id)is not { CardId: 0, CardKind: CardKind.Slash } declaration || declaration.SourceSeat != action.ActorSeat || history.OfType<CardActionAcceptedEvent>().Select(e => e.Action).FirstOrDefault(a => a.ActionId == action.ActionId)is not { Type: CardActionType.Use, EffectiveKind: CardKind.Slash, PhysicalCards.Count: 0 } accepted || accepted.ActorSeat != action.ActorSeat || accepted.ProviderSeat != action.ProviderSeat || !accepted.TargetSeats.SequenceEqual(declared.TargetSeats) || !accepted.ConversionChain.SequenceEqual(action.ConversionChain))
            return false;
        var original = declared.TargetSeats[0];
        var paused = ProgramInstructionResolver.Default.Resolve(parent, program).GetPausedInstruction(parent.InstructionIndex).Effect;
        var exact = paused.Op switch
        {
            SkillProgramEffectOp.OfferVirtualSlashOrDraw => parent.SelectedTargetSeats.SequenceEqual([action.ActorSeat]) && action.ConversionChain.Count == 0,
            SkillProgramEffectOp.UseVirtualSlash => parent.OwnerSeat == action.ActorSeat && action.ConversionChain.SequenceEqual([new CardConversionSource(parent.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent), parent.OwnerSeat, parent.SkillInstanceId)]) && (paused.TargetReference is null ? parent.SelectedTargetSeats.SequenceEqual([original]) : paused.TargetReference.Kind == ProgramParticipantRef.EventTarget && parent.WindowContext?.TargetSeat == original),
            SkillProgramEffectOp.UseVirtualCard or SkillProgramEffectOp.OfferUnlimitedVirtualSlash => parent.OwnerSeat == action.ActorSeat && parent.SelectedTargetSeats.SequenceEqual([original]) && action.ConversionChain.Count == 0 && paused.UseCardActionWindows && paused.OutputKind == CardKind.Slash && paused.TargetRestriction == SkillProgramCardTargetRestriction.DistanceUnlimitedAgainstTarget && (parent.TriggerId is not null || paused.Op == SkillProgramEffectOp.OfferUnlimitedVirtualSlash),
            _ => false
        };
        if (!exact)
            return false;
        // Preserve the mature producer's one primary post-redirection return.
        primary = Array.AsReadOnly(new[] { use.TargetSeats[0] });
        return true;
    }
}
