#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOverflowTargetCancellationRuntime
{
    internal const string OverflowMaterialReason = "skill-program.overflow-target-cancellation.give";
    internal long OverflowMovementSequence => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;

    internal static IReadOnlyList<int> OverflowPrimaryTargets(CardKind kind, IReadOnlyList<int> targets) => RuntimeSkills.RuntimeRules_Core_OverflowTargetCancellation.OverflowPrimaryTargets(kind, targets);
    internal static IReadOnlyList<int> OverflowRetainedTargets(CardKind kind, int owner, IReadOnlyList<int> targets) => RuntimeSkills.RuntimeRules_Core_OverflowTargetCancellation.OverflowRetainedTargets(kind, owner, targets);
    internal bool TryGetOverflowTargetUse(int owner, ProgramSkillWindowContext context, out CardUseFrame use, out ProgramCardTriggerWindowFrame window)
    {
        use = null !;
        window = null !;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } identity } || context.OwnerSeat != owner || identity.ActorSeat != owner || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } found || found.ParentFrameId != identity.ParentCardUseFrameId || found.CompletedResponseReturn is not null || found.Continuation is not (ProgramCardContinuation.Slash or ProgramCardContinuation.FinalizedTrick or ProgramCardContinuation.FinalizedSimpleCard or ProgramCardContinuation.DelayedCard or ProgramCardContinuation.FinalizedDelayedPlacement) || CoreGameEngineCardActionsRuntime.GetCardActionWindow(found) != SkillProgramTriggerWindow.CardUseTargetsFinalized || _coreCardUseLifecycleOperations.LifecycleCardUse(found.ParentFrameId)is not { Action: { Type: CardActionType.Use } action } actual || action.ActionId != identity.CardActionId || action.ActorSeat != owner || actual.SourceSeat != owner || identity.EffectiveKind != actual.CardKind || action.ResponderSeat is not null || action.OpponentSeat is not null || action.EffectiveKind != actual.CardKind || found.Action.ActionId != action.ActionId || found.Action.ActorSeat != owner || !found.Action.TargetSeats.SequenceEqual(actual.TargetSeats) || !action.TargetSeats.SequenceEqual(actual.TargetSeats) || actual.TargetSeats.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || action.PhysicalCards.Any(c => c.CardId <= 0) || action.PhysicalCards.DistinctBy(c => c.CardId).Count() != action.PhysicalCards.Count || actual.CardKind == CardKind.BorrowedSword && actual.TargetSeats.Count % 2 != 0)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == found.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != actual.Id)
            return false;
        use = actual;
        window = found;
        return true;
    }

    internal bool ExactOverflowTargetParent(ProgramSkillFrame frame, out CardUseFrame use, out ProgramCardTriggerWindowFrame window)
    {
        use = null !;
        window = null !;
        if (frame.TriggerId is null || frame.InstructionIndex != 1 || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats.Count != 0 || frame.WindowContext is not { } context || !TryGetOverflowTargetUse(frame.OwnerSeat, context, out use, out window) || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(new(frame.OwnerSeat, frame.SkillId, frame.TriggerId, frame.SkillInstanceId, frame.GameplayHash, 0), context, window))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != window.Id || !window.Activated || _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program is not { } program || program.GameplayHash != frame.GameplayHash || program.Triggers.SingleOrDefault(t => t.Id == frame.TriggerId)is not { } trigger)
            return false;
        OverflowTargetCancellationContract.ValidateTrigger(frame.SkillId, trigger);
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == frame.Id && e.OwnerSeat == frame.OwnerSeat && e.SkillId == frame.SkillId && e.BindingId == frame.TriggerId && e.SkillInstanceId == frame.SkillInstanceId && e.Window == context.Window) == 1;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 990)]
    internal bool CanOfferOverflowTargetCancellation(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.CancelOtherCurrentUseTargetsIfOverHandLimit))
            return true;
        return TryGetOverflowTargetUse(candidate.OwnerSeat, context, out var use, out var window) && _coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(candidate, context, window) && OverflowPrimaryTargets(use.CardKind, use.TargetSeats).Any(s => s != candidate.OwnerSeat) && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count > _coreNativeRuleQueriesOperations.GetHandLimit(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]) && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<OverflowUseTargetsCanceledEvent>().Any(e => e.Receipt.CardUseFrameId == use.Id && e.Receipt.ActionId == use.Action!.ActionId && e.Receipt.Source == new CardConversionSource(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId));
    }

    internal SkillProgramStepOutcome BeginOverflowTargetCancellation(ProgramSkillFrame input)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (!ExactOverflowTargetParent(frame, out var use, out var window) || frame.OverflowTargetCancellation is not null || frame.PendingMovementContinuation is not null || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<OverflowUseTargetsCanceledEvent>().Any(e => e.ProgramFrameId == frame.Id))
            throw new InvalidOperationException("Overflow cancellation lost its exact unpaid designation instruction.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count;
        var limit = _coreNativeRuleQueriesOperations.GetHandLimit(owner);
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, frame.SkillId, frame.SkillInstanceId) || hand <= limit || !OverflowPrimaryTargets(use.CardKind, use.TargetSeats).Any(s => s != frame.OwnerSeat))
            return SkillProgramStepOutcome.Continue;
        if (!_coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, use.Action!))
            throw new InvalidOperationException("Overflow cancellation cannot replace an unproven actual use.");
        var originals = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TargetsConfirmedEvent>().Where(e => e.ResolutionId == use.Id).ToArray();
        if (originals is not [var original] || original.TargetSeats.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || use.CardKind == CardKind.BorrowedSword && original.TargetSeats.Count % 2 != 0)
            throw new InvalidOperationException("Overflow cancellation lost the original designation.");
        var originalPrimary = OverflowPrimaryTargets(use.CardKind, original.TargetSeats);
        int? recipient = originalPrimary is [var unique] && unique != frame.OwnerSeat ? unique : null;
        var beforeAction = use.Action!;
        var retained = OverflowRetainedTargets(use.CardKind, frame.OwnerSeat, use.TargetSeats);
        var materials = recipient is { } recipientSeat && _coreMatchParticipantsPlayersAccess._players[recipientSeat].IsAlive ? beforeAction.PhysicalCards.Select(c => c.CardId).Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing && !_coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(use.Id, id)).ToArray() : [];
        var receipt = new OverflowTargetCancellationReceipt(1, new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, window.Id, use.Id, beforeAction.ActionId, beforeAction.ProviderSeat, use.CardKind, beforeAction, hand, limit, recipient, OverflowMovementSequence, OverflowMovementSequence, null)
        {
            OriginalTargetSeats = original.TargetSeats,
            BeforeTargetSeats = use.TargetSeats,
            CanceledPrimaryTargetSeats = OverflowPrimaryTargets(use.CardKind, use.TargetSeats).Where(s => s != frame.OwnerSeat).ToArray(),
            ResultTargetSeats = retained,
            MaterialCardIds = materials
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { OverflowTargetCancellation = receipt, PendingMovementContinuation = materials.Length > 0 ? new(frame.OwnerSeat, 0, null) : null });
        var updated = _coreDesignatedExtraTargetsOperations.CloneDesignatedExtraTargetAction(beforeAction, retained);
        _coreProgramCardUseRolesOperations.UpdateProgramRoleCardUse(use with { TargetSeats = retained, TargetIndex = 0, Action = updated, TargetsAdjusted = true, SlashTargetsCancelled = CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && retained.Count == 0, SequentialTrick = retained.Count == 0 ? null : use.SequentialTrick, PreparedTargetAttacks = use.PreparedTargetAttacks is { } prepared ? Array.AsReadOnly(prepared.Where(attack => retained.Contains(attack.TargetSeat)).ToArray()) : null, Continuations = use.Continuations with { FangtianHalberd = use.Continuations.FangtianHalberd is { Active: true } multi ? multi with { TargetSeats = retained } : use.Continuations.FangtianHalberd } }, updated);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OverflowUseTargetsCanceledEvent(frame.Id, receipt));
        if (materials.Length > 0)
        {
            var destination = CardLocation.Hand(recipient!.Value);
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(materials, destination, new(OverflowMaterialReason), (batchId, _) =>
            {
                receipt = receipt with
                {
                    SequenceAfter = OverflowMovementSequence,
                    MovementBatchId = batchId,
                    MovementIssued = true
                };
            });
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { OverflowTargetCancellation = receipt });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OverflowUseMaterialTransferIssuedEvent(frame.Id, use.Id, beforeAction.ActionId, recipient.Value, materials.Length, receipt.SequenceBefore, receipt.SequenceAfter, receipt.MovementBatchId));
        }

        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainOverflowTargetChildren(ProgramSkillFrame frame) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frame.Id);
    [ProgramContinuation(ProgramContinuationKind.Resume, 130)]
    internal bool ResumeOverflowTargetCancellation(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != id || frame.OverflowTargetCancellation is not { } receipt)
            return false;
        if (DrainOverflowTargetChildren(frame))
            return true;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OverflowTargetCancellationCompletedEvent(frame.Id, receipt.CardUseFrameId, receipt.ActionId));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { OverflowTargetCancellation = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 110)]
    internal bool ReturnOverflowTargetCancellation(ProgramSkillFrame frame)
    {
        if (frame.OverflowTargetCancellation is null || frame.PendingMovementContinuation is null)
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 580)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 600)]
    internal bool CanContinueOverflowTargetCancellation(ProgramSkillFrame frame) => frame.OverflowTargetCancellation is not null && _coreOverflowTargetCancellationObserversOperations.ValidOverflowTargetCancellationReceipt(frame);
    internal bool HasOverflowTargetCancellation(CardUseFrame use) => use.Action is { } action && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<OverflowUseTargetsCanceledEvent>().Any(e => e.Receipt.CardUseFrameId == use.Id && e.Receipt.ActionId == action.ActionId && _coreOverflowTargetCancellationObserversOperations.IsOverflowTargetCancellationFact(use, e, e.Receipt.BeforeTargetSeats));
    internal bool IsOverflowTargetCancellationPhysicalClaim(long useId, int cardId) => _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { Action: { } action } use && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<OverflowUseTargetsCanceledEvent>().Any(e => e.Receipt.CardUseFrameId == useId && e.Receipt.ActionId == action.ActionId && e.Receipt.MaterialCardIds.Contains(cardId) && _coreOverflowTargetCancellationObserversOperations.IsOverflowTargetCancellationFact(use, e, e.Receipt.BeforeTargetSeats) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<OverflowUseMaterialTransferIssuedEvent>().Any(m => m.ProgramFrameId == e.ProgramFrameId && _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(move => move.CardId == cardId && move.Sequence > m.SequenceBefore && move.Sequence <= m.SequenceAfter && move.From == CardLocation.Processing && (move.To == CardLocation.Hand(m.RecipientSeat) || _coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRedirectedGain(move, CardLocation.Hand(m.RecipientSeat))) && move.Reason.Value == OverflowMaterialReason) == 1));
    internal bool TryCompleteOverflowCanceledUse(ProgramCardTriggerWindowFrame window)
    {
        if (window.Continuation is not (ProgramCardContinuation.Slash or ProgramCardContinuation.FinalizedTrick or ProgramCardContinuation.FinalizedSimpleCard or ProgramCardContinuation.DelayedCard or ProgramCardContinuation.FinalizedDelayedPlacement) || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { TargetSeats.Count: 0 } use || !(_characterCuiLieAnniversaryOperations.HasAnniversaryCuiLieCancellation(use) || _characterXunYuMobileGodOperations.HasDinghanCancellation(use) || _characterHuaManOlOperations.HasHuaManSouyingCancellation(use) || HasOverflowTargetCancellation(use) || use.BeiMiHuTargetMutation is { Kind: "cancel" } && _characterBeiMiHuOlOperations.ValidBeiMiHuTargetMutation(use) || window.Continuation == ProgramCardContinuation.FinalizedTrick && (_coreRoundGainedTrickTargetsOperations.Value.HasRoundGainedTrickTargetCancellation(use) || _characterYuanTanYuanShangOlOperations.HasNeifaTrickTargetCancellation(use))))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id || window.Action.ActionId != use.Action!.ActionId)
            throw new InvalidOperationException("Overflow cancellation lost its whole-use native return.");
        if (window.Continuation == ProgramCardContinuation.Slash)
        {
            if (window.AttackOwnerFrameId != use.Id || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not { } attack || attack.ResolutionId != use.Id)
                throw new InvalidOperationException("Overflow cancellation lost its exact Slash attack.");
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
        }
        else
        {
            var representation = use.CardId == 0 ? new Card(0, use.CardKind, Suit.None, 0) : _coreResponseEntityExchangeOperations.EntityAtCurrentLocation(use.CardId);
            _coreNativeCardUseCompletionOperations.FinishCardUse(use.Id, representation, use.CardKind);
        }

        return true;
    }

    SkillProgramStepOutcome IOverflowTargetCancellationHost.BeginOverflowTargetCancellation(ProgramSkillFrame frame) => BeginOverflowTargetCancellation(frame);
}
