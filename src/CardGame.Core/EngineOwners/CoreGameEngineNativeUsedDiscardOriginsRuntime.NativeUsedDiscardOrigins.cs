#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNativeUsedDiscardOriginsRuntime
{
    internal void CaptureClaimedDeclarationTrueUse(DeclaredCardPayment paid)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.RequiresFact(ProgramTrackedFact.NativeTrueUseProcessingEntry) || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { Action: { Type: CardActionType.Use } action } use || paid.OwnerFrameId == use.Id || use.CardKind != action.EffectiveKind || action.ActorSeat != paid.ActorSeat || action.ProviderSeat != paid.ProviderSeat || !action.PhysicalCards.Contains(paid.Cost) || !action.ConversionChain.Contains(paid.Source) || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == paid.OwnerFrameId)?.AcceptedDeclarationPayment is not { Claimed: true } claimed || claimed != paid with
        {
            Claimed = true
        }

        )
            return;
        var entered = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(m => m.CardId == paid.Cost.CardId);
        if (entered is null || entered.From != paid.Cost.From || entered.To != CardLocation.Processing || entered.CardKind != paid.Cost.CardKind || entered.Reason.Value != "conversion.declaration.pay" || !_coreCardSupplyCompletionOperations.IsCardSupplyCompletionEntry(action, use.Id, paid.OwnerFrameId, paid.Cost, entered))
            throw new InvalidOperationException("A native declared use must claim its original exact successful declaration payment.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NativeTrueUseProcessingEntryEvent(use.Id, action.ActionId, entered.CardId, entered.CardKind, entered.From, entered.Sequence, action.ProviderSeat, NativeTrueUseDiscardKind.Use));
    }

    internal void CaptureNativeTrueUseProcessingEntries(CardMovementBatchContext batch)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.RequiresFact(ProgramTrackedFact.NativeTrueUseProcessingEntry) || batch.ParentFrameId is not { } parentId || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == parentId)is not { } parent)
            return;
        foreach (var move in batch.Movements)
        {
            if (move.To != CardLocation.Processing || move.From == move.To || move.Sequence <= 0 || move.Sequence > _coreCardMovementJournalCardMovementsAccess._cardMovements.Count || _coreCardMovementJournalCardMovementsAccess._cardMovements[(int)move.Sequence - 1] != move)
                continue;
            NativeTrueUseDiscardKind kind;
            int provider;
            long parentAction;
            if (parent is CardUseFrame { Action: { Type: CardActionType.Use } action } use && (move.Reason == CardMoveReasons.Use || use.CardKind == CardKind.IronChain && move.Reason == CardMoveReasons.IronChainUse) && action.EffectiveKind == use.CardKind && action.PhysicalCards.Any(cost => cost.CardId == move.CardId && cost.CardKind == move.CardKind && cost.From == move.From))
            {
                kind = NativeTrueUseDiscardKind.Use;
                parentAction = action.ActionId;
                provider = action.ProviderSeat;
            }
            else if (parent is NullificationWindowFrame counterspell && (move.Reason == CardMoveReasons.Nullification || move.Reason == CardMoveReasons.Respond) && move.From.OwnerSeat is { } counterspellProvider && counterspell.CandidateIndex >= 0 && counterspell.CandidateIndex < counterspell.CandidateSeats.Count && counterspell.CandidateSeats[counterspell.CandidateIndex] == counterspellProvider && _coreDelayedNullificationOperations.HasNullificationEffectOwner(counterspell))
            {
                kind = NativeTrueUseDiscardKind.Nullification;
                parentAction = _coreDelayedNullificationOperations.NullificationParentActionId(counterspell) ?? 0;
                provider = counterspellProvider;
            }
            else if (parent is CardUseFrame { Action: { } incoming, CardAttack: { Active: true } attack } && move.Reason == CardMoveReasons.Respond && _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is null && (move.From.OwnerSeat == attack.TargetSeat || move.From == CardLocation.DrawPile && parent.RequestedDeckBasicMaterial is { Claiming: true, Intent: RequestedDeckBasicIntent.Dodge } requested && requested.OwnerFrameId == parentId && requested.ActorSeat == attack.TargetSeat && requested.SelectedCardId == move.CardId && requested.SelectedKind == move.CardKind && requested.PaidMovementSequence == move.Sequence && _coreRequestedDeckBasicOwnershipOperations.HasValidRequestedDeckBasicMaterial(parent)) && CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Dodge))
            {
                kind = NativeTrueUseDiscardKind.Dodge;
                parentAction = incoming.ActionId;
                provider = attack.TargetSeat;
            }
            else
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NativeTrueUseProcessingEntryEvent(parentId, parentAction, move.CardId, move.CardKind, move.From, move.Sequence, provider, kind));
        }
    }

    internal static bool IsNativeUseFinishReason(CardMoveReason reason) => RuntimeSkills.RuntimeRules_Core_NativeUsedDiscardOrigins.IsNativeUseFinishReason(reason);
    internal bool IsNativeTrueUseDiscardMovement(CardMovementBatchContext batch, CardMovementRecord move)
    {
        if (move.To != CardLocation.DiscardPile || move.From != CardLocation.Processing || batch.ParentFrameId is not { } parentId || !batch.Movements.Contains(move) || move.Sequence <= 0 || move.Sequence > _coreCardMovementJournalCardMovementsAccess._cardMovements.Count || _coreCardMovementJournalCardMovementsAccess._cardMovements[(int)move.Sequence - 1] != move)
            return false;
        var entered = _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault(m => m.CardId == move.CardId && m.Sequence < move.Sequence);
        if (entered is null || entered.To != CardLocation.Processing || entered.From == entered.To || entered.CardKind != move.CardKind)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == parentId);
        var facts = _coreProgramEventHistoryOperations.ProgramEventHistory<NativeTrueUseProcessingEntryEvent>().Where(e => e.ParentFrameId == parentId && e.CardId == move.CardId && e.CardKind == move.CardKind && e.EntrySequence == entered.Sequence && e.From == entered.From).ToArray();
        if (facts is [var fact])
        {
            if (fact.Kind == NativeTrueUseDiscardKind.Use)
                return IsNativeUseFinishReason(move.Reason) && parent is CardUseFrame { Action: { Type: CardActionType.Use } action } use && action.ActionId == fact.ParentActionId && action.EffectiveKind == use.CardKind && action.ProviderSeat == fact.ProviderSeat && action.PhysicalCards.Any(cost => cost.CardId == move.CardId && cost.CardKind == move.CardKind && cost.From == entered.From);
            if (fact.Kind == NativeTrueUseDiscardKind.Dodge)
                return move.Reason == CardMoveReasons.ResponseFinished && parent is CardUseFrame { Action: { } incoming, CardAttack: { Active: true } attack } && incoming.ActionId == fact.ParentActionId && attack.TargetSeat == fact.ProviderSeat && CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Dodge);
            return fact.Kind == NativeTrueUseDiscardKind.Nullification && (move.Reason == CardMoveReasons.NullificationFinished || entered.Reason == CardMoveReasons.Respond && move.Reason == CardMoveReasons.ResponseFinished) && parent is NullificationWindowFrame counterspell && _coreDelayedNullificationOperations.HasNullificationEffectOwner(counterspell) && (_coreDelayedNullificationOperations.NullificationParentActionId(counterspell) ?? 0) == fact.ParentActionId;
        }

        if (facts.Length != 0 || entered.Reason.Value != "conversion.declaration.pay")
            return false;
        // A successful declaration already paid before the native parent claimed
        // it. Reuse that exact claimed invoice; failed declarations are not uses.
        if (parent?.AcceptedDeclarationPayment is not { Claimed: true } paid || paid.OwnerFrameId != parentId || paid.Cost.CardId != move.CardId || paid.Cost.CardKind != move.CardKind || paid.Cost.From != entered.From || paid.ProviderSeat != entered.From.OwnerSeat)
            return false;
        if (parent is CardUseFrame { Action: { Type: CardActionType.Use } declared } declaredUse && declared.PhysicalCards.Contains(paid.Cost))
            return IsNativeUseFinishReason(move.Reason) && declared.EffectiveKind == declaredUse.CardKind && paid.ActorSeat == declared.ActorSeat && paid.ProviderSeat == declared.ProviderSeat && declared.ConversionChain.Contains(paid.Source);
        if (parent is NullificationWindowFrame pending)
            return paid.DeclaredKind == CardKind.Nullification && paid.ActorSeat == paid.ProviderSeat && move.Reason == CardMoveReasons.NullificationFinished && _coreDelayedNullificationOperations.HasNullificationEffectOwner(pending);
        if (parent is not CardUseFrame { CardAttack: { Active: true } slash } || !CoreNativeCardClassificationsRuntime.IsSlashCard(slash.EffectiveCardKind ?? CardKind.Dodge) || slash.TargetSeat != paid.ProviderSeat || paid.ActorSeat != paid.ProviderSeat || paid.DeclaredKind != CardKind.Dodge || move.Reason != CardMoveReasons.ResponseFinished)
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().Any(e => _coreResponseUseCompletionOperations.IsCompletedResponseUse(e.Action, ProgramCardContinuation.Dodge) && e.Action.PhysicalCards.Contains(paid.Cost) && e.Action.ConversionChain.Contains(paid.Source) && _coreResponseCompletionOperations.IsResponseCompletionEntry(e.Action, ProgramCardContinuation.Dodge, parentId, paid.Cost, entered));
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 4000)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureNativeTrueUseProcessingEntries(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureNativeTrueUseProcessingEntries(completed);
        return completed;
    }
}
