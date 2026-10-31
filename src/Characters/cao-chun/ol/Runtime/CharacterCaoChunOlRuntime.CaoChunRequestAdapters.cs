#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoChunOlRuntime
{
    internal const string CaoChunFactionBind = "shanjia-initial-faction";
    internal bool ValidCaoChunPaidActivation(ProgramSkillFrame f)
    {
        if (f.CaoChun is not { } r || f.ActivationId != "draw-and-slash" || f.InstructionIndex != 1 || r.Source != CaoChunSource(f) || r.GameplayHash != f.GameplayHash || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.PhaseInstance != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId)
            return false;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ShanjiaMovementPaidEvent>().Where(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Stage == CaoChunStage.DrawChildren).ToArray();
        return facts is [var draw] && draw.Before <= draw.After && draw.After <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && draw.ActualCount is >= 0 and <= 3 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > draw.Before && m.Sequence <= draw.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == ShanjiaDrawReason) == draw.ActualCount;
    }

    internal bool IsCaoChunInitialRequest(ProgramSkillFrame f) => f.CaoChun?.Stage == CaoChunStage.InitialSlash && ValidCaoChunPaidActivation(f) && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == f.OwnerSeat;
    internal bool CaoChunInitialAdapterActor(ProgramSkillFrame f, int actor, int? target, int cursor, long? parent, long? request) => f.Id == parent && f.Id == request && f.InstructionIndex == cursor && actor == f.OwnerSeat && target is { } seat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && seat != actor && ValidCaoChunPaidActivation(f) && f.CaoChun is { Stage: CaoChunStage.InitialSlash or CaoChunStage.NativeSlash } r && (r.Slash is null || r.Slash.TargetSeat == seat);
    internal void PrepareCaoChunDrawFundedSlash(ProgramSkillFrame f, DrawFundedDistinctBasicPayment p)
    {
        if (!IsCaoChunInitialRequest(f) || !_coreDrawFundedDistinctBasicPaymentOperations.ValidDrawFundedDistinctBasicPayment(p) || !CaoChunInitialAdapterActor(f, p.Source.OwnerSeat, p.TargetSeat, p.Cursor, p.ParentFrameId, p.RequestFrameId) || p.Intent != DrawFundedDistinctBasicIntent.AssistedSlash)
            throw new InvalidOperationException("Shanjia draw-funded initial Slash lost its exact native paid need.");
        var r = f.CaoChun!;
        var n = new CaoChunNativeSlash(f.Id, r.Source, r.GameplayHash, r.InstructionIndex, r.ActualTurn, r.PhaseInstance, false, p.TargetSeat!.Value, p.EffectiveKind)
        {
            DrawFundedPayment = p,
            PaymentBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { CaoChun = r with { Stage = CaoChunStage.NativeSlash, Slash = n } });
    }

    internal void PrepareCaoChunChainedSlash(ProgramSkillFrame f, ChainedStateBasicPayment p)
    {
        if (!IsCaoChunInitialRequest(f) || !_coreChainedStateBasicConversionsOperations.ValidChainedStateBasicPayment(p) || !CaoChunInitialAdapterActor(f, p.Source.OwnerSeat, p.TargetSeat, p.Cursor, p.ParentFrameId, p.RequestFrameId) || p.Intent != ChainedStateBasicIntent.AssistedSlash)
            throw new InvalidOperationException("Shanjia chained initial Slash lost its actual paid state-change need.");
        var r = f.CaoChun!;
        var n = new CaoChunNativeSlash(f.Id, r.Source, r.GameplayHash, r.InstructionIndex, r.ActualTurn, r.PhaseInstance, false, p.TargetSeat!.Value, p.EffectiveKind)
        {
            ChainedPayment = p,
            PaymentBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { CaoChun = r with { Stage = CaoChunStage.NativeSlash, Slash = n } });
    }

    internal void BeginCaoChunFactionSlash(ProgramSkillFrame f, int target)
    {
        if (!IsCaoChunInitialRequest(f) || !CaoChunSourceCurrent(f) || _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not null || !_coreAssistedFactionSlashOperations.CanRequestAssistedProgramFactionSlash(f.OwnerSeat, target))
            throw new InvalidOperationException("Shanjia faction request lost its current exact native need.");
        var policy = _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], CardKind.Slash)!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoChun = f.CaoChun!with { Faction = new(f.Id, target, policy) } });
        var seats = _coreNativeEquipmentAndFactionQueriesOperations.GetFactionProviderSeats(f.OwnerSeat, policy.FactionId);
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = _kernelCoreMainOperations.CreateFactionCardRequestHandle(f.Id, FactionCardRequestPurpose.AssistedProgramUse, f.OwnerSeat, seats, policy.FactionId, targetSeat: target, programSkillFrameId: f.Id, policySource: policy, assistedResultBind: CaoChunFactionBind);
        _coreMatchOutcomeStatusAccess._status = EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashRequestedEvent(f.Id, f.OwnerSeat, seats, policy.SkillId, IsActiveUse: true, target));
        _coreNativeFactionSlashOperations.AdvanceFactionSlashCandidate();
    }

    internal bool ValidateCaoChunFactionSlashParent(NativeOwners.FactionCardRequestHandle pending, ProgramSkillFrame f)
    {
        if (f.CaoChun?.Faction is not { } issued)
            return false;
        return true;
    }

    internal bool PrepareCaoChunFactionNative(ProgramSkillFrame f, NativeOwners.FactionCardRequestHandle pending, CharacterState provider, IReadOnlyList<Card> cards, CardKind kind)
    {
        if (!ValidateCaoChunFactionSlashParent(pending, f))
            return false;
        var r = f.CaoChun!;
        var native = new CaoChunNativeSlash(f.Id, r.Source, r.GameplayHash, r.InstructionIndex, r.ActualTurn, r.PhaseInstance, false, pending.TargetSeat!.Value, kind)
        {
            ProviderSeat = provider.Seat,
            Materials = cards.Select(c => new CardActionCost(c.Id, c.Kind, _cardZonesOperations._cardZones.GetLocation(c.Id))).ToArray(),
            PaymentBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { CaoChun = r with { Stage = CaoChunStage.NativeSlash, Slash = native } });
        return true;
    }

    internal bool FinishCaoChunFactionDeclined(long id, NativeOwners.FactionCardRequestHandle pending)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.CaoChun?.Faction is null)
            return false;
        ValidateCaoChunFactionSlashParent(pending, f);
        FinishCaoChun(f);
        return true;
    }

    internal void DispatchCaoChunRequestedDeckSlash(ProgramSkillFrame f, Card card, IReadOnlyList<int> targets)
    {
        if (!IsCaoChunInitialRequest(f) || f.RequestedDeckBasicMaterial is not { Claiming: true, SelectedCardId: { } id } material || id != card.Id || material.ActorSeat != f.OwnerSeat || material.Intent != RequestedDeckBasicIntent.AssistedSlash || !_coreRequestedDeckBasicOwnershipOperations.HasValidRequestedDeckBasicMaterial(f) || targets is not [var target] || !_coreNativeTargetLegalityOperations.CanUseSlashTarget(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[target], card, effectiveKind: card.Kind, physicalCardIds: [card.Id]))
            throw new InvalidOperationException("Shanjia deck request lost its exact selected real native material.");
        var r = f.CaoChun!;
        var n = new CaoChunNativeSlash(f.Id, r.Source, r.GameplayHash, r.InstructionIndex, r.ActualTurn, r.PhaseInstance, false, target, card.Kind)
        {
            Materials = [new(card.Id, card.Kind, CardLocation.DrawPile)],
            PaymentBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { CaoChun = r with { Stage = CaoChunStage.NativeSlash, Slash = n } });
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeSlashUseOperations.ResolveSlashCore(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[target], card, card.Kind, f.OwnerSeat, countsTowardSlashLimit: false, programSkillCardUseFrameId: f.Id);
    }
}
