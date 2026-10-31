#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRecipientContestReceiptsRuntime
{
    internal bool ValidUniqueHpPeer(ProgramSkillFrame f)
    {
        if (f.UniqueHpPeer is not { } r || r.InstructionIndex != f.InstructionIndex || f.InstructionIndex != 1 || r.Source != _coreUniqueHpPeerProgramsOperations.Value.RecipientContestSource(f) || r.GameplayHash != f.GameplayHash || r.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || !_coreUniqueHpPeerProgramsOperations.Value.ExactUniqueHpPeerParent(f) || f.WindowContext!.ParentFrameId != r.WindowFrameId || f.WindowContext.ActualUseTarget != r.TargetIdentity || r.TargetIdentity.ActionId != r.CardActionId || r.TargetIdentity.CardUseFrameId != r.CardUseFrameId || !Enum.IsDefined(r.Stage) || string.IsNullOrWhiteSpace(r.Source.BindingId) || string.IsNullOrWhiteSpace(r.Source.SkillInstanceId) || r.ChooserSeat != f.OwnerSeat && r.ChooserSeat != r.PeerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.ChooserSeat) || r.RequiredCount is < 0 or > 2 || r.SelectedCardIds.Count != r.SelectedLocations.Count || r.SelectedCardIds.Count > r.RequiredCount || r.SelectedCardIds.Distinct().Count() != r.SelectedCardIds.Count || r.SelectedLocations.Any(l => l.OwnerSeat != r.ChooserSeat || l.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)) || r.Stage != UniqueHpPeerStage.SelectingCost && r.SelectedCardIds.Count != 0 || ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions is not [{ Op: SkillProgramEffectOp.DiscardDrawAndOfferUniqueHpPeer }])
            return false;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<UniqueHpPeerStartedEvent>().Count(e => e == new UniqueHpPeerStartedEvent(f.Id, r.Source, r.GameplayHash, r.TurnNumber, r.TurnOwnerSeat, r.WindowFrameId, r.CardUseFrameId, r.CardActionId, r.TargetIdentity)) != 1)
            return false;
        if (r.PeerSeat is { } peer)
        {
            if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(peer) || peer == f.OwnerSeat || r.PeerHp is null || r.OwnerInvoice is not { DrawIssued: true } || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<UniqueHpPeerOfferedEvent>().Count(e => e.FrameId == f.Id && e.PeerSeat == peer && e.Hp == r.PeerHp) != 1)
                return false;
        }
        else if (r.PeerHp is not null || r.PeerInvoice is not null || r.Stage == UniqueHpPeerStage.PeerOffer)
            return false;
        if (r.OwnerInvoice is { } own && !ValidUniqueHpPeerInvoice(f, own, f.OwnerSeat) || r.PeerInvoice is { } other && (r.PeerSeat is not { } seat || !ValidUniqueHpPeerInvoice(f, other, seat)))
            return false;
        var invoice = r.ChooserSeat == f.OwnerSeat ? r.OwnerInvoice : r.PeerInvoice;
        if (r.Stage is UniqueHpPeerStage.CostChildren or UniqueHpPeerStage.DrawChildren)
            return invoice is not null && invoice.RequiredCount == r.RequiredCount && (r.Stage == UniqueHpPeerStage.DrawChildren) == invoice.DrawIssued && f.PendingMovementContinuation is { BeforeCount: 0, CoverageResultBind: null } pending && pending.SubjectSeat == r.ChooserSeat;
        if (f.PendingMovementContinuation is not null || r.Stage == UniqueHpPeerStage.SelectingCost && (r.ChooserSeat == f.OwnerSeat ? r.OwnerInvoice is not null : r.PeerInvoice is not null))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && r.Stage is UniqueHpPeerStage.SelectingCost or UniqueHpPeerStage.PeerOffer)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } p || p.PlayerSeat != r.ChooserSeat || p.TargetSeat != r.ChooserSeat || p.SkillPrompt?.SkillId != f.SkillId || p.Choices.Count == 0 || p.Choices.Count != _coreUniqueHpPeerProgramsOperations.Value.UniqueHpPeerChoices(f).Count || p.Choices.Any(c => !_coreUniqueHpPeerProgramsOperations.Value.UniqueHpPeerChoices(f).Any(real => real.Id == c.Id && real.Cards.SequenceEqual(c.Cards) && real.Targets.SequenceEqual(c.Targets) && real.Parameters.Count == c.Parameters.Count && real.Parameters.All(kv => c.Parameters.GetValueOrDefault(kv.Key) == kv.Value))))
                return false;
        }

        return true;
    }

    internal bool ValidUniqueHpPeerInvoice(ProgramSkillFrame f, UniqueHpPeerInvoice r, int seat)
    {
        if (r.ActorSeat != seat || r.RequiredCount is < 0 or > 2 || r.CardIds.Count != r.RequiredCount || r.SourceLocations.Count != r.CardIds.Count || r.CardIds.Distinct().Count() != r.CardIds.Count || r.SourceLocations.Any(l => l.OwnerSeat != seat || l.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)) || r.CostBefore < 0 || r.CostAfter < r.CostBefore || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<UniqueHpPeerCostPaidEvent>().Count(e => e == new UniqueHpPeerCostPaidEvent(f.Id, seat, r.RequiredCount, r.CostBefore, r.CostAfter)) != 1)
            return false;
        foreach (var pair in r.CardIds.Select((id, i) => (id, from: r.SourceLocations[i])))
            if (_coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.CostBefore && m.Sequence <= r.CostAfter && m.CardId == pair.id && m.From == pair.from && m.Reason.Value == CoreGameEngineUniqueHpPeerProgramsRuntime.UniqueHpPeerCostReason(f) && (m.To == CardLocation.DiscardPile || pair.from.Zone == CardZoneKind.Equipment && m.To == CardLocation.OutsideGame && (_coreAdvancedLifecycleOperations.GetAdvancedCard(pair.id).IsGeneralWeapon || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(m)))) != 1)
                return false;
        if (!r.DrawIssued)
            return r.DrawBefore == 0 && r.DrawAfter == 0 && r.ActualDrawCount == 0 && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<UniqueHpPeerDrawIssuedEvent>().Any(e => e.FrameId == f.Id && e.ActorSeat == seat);
        return r.DrawBefore >= r.CostAfter && r.DrawAfter >= r.DrawBefore && r.ActualDrawCount is >= 0 and <= 2 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<UniqueHpPeerDrawIssuedEvent>().Count(e => e == new UniqueHpPeerDrawIssuedEvent(f.Id, seat, r.DrawBefore, r.DrawAfter, r.ActualDrawCount)) == 1 && _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.DrawBefore && m.Sequence <= r.DrawAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(seat) && m.Reason.Value == CoreGameEngineUniqueHpPeerProgramsRuntime.UniqueHpPeerDrawReason(f, seat)) == r.ActualDrawCount;
    }

    internal bool ValidRecipientContest(ProgramSkillFrame f)
    {
        if (f.RecipientContest is not { } r || f.TriggerId is not null || f.WindowContext is not null || r.InstructionIndex != 1 || f.InstructionIndex is < 1 or > 2 || f.InstructionIndex == 2 && r.Stage is RecipientContestStage.GiftChildren or RecipientContestStage.ChoosingThird or RecipientContestStage.Pindian || r.Source != _coreUniqueHpPeerProgramsOperations.Value.RecipientContestSource(f) || r.GameplayHash != f.GameplayHash || r.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || f.SelectedTargetSeats is not [var recipient] || recipient != r.RecipientSeat || recipient == f.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || !Enum.IsDefined(r.Stage) || r.GiftCardIds.Count == 0 || r.GiftCardIds.Any(id => id <= 0) || r.GiftCardIds.Distinct().Count() != r.GiftCardIds.Count || r.SequenceBefore < 0 || r.SequenceAfter <= r.SequenceBefore || string.IsNullOrWhiteSpace(r.Source.BindingId) || string.IsNullOrWhiteSpace(r.Source.SkillInstanceId) || ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions is not [{ Op: SkillProgramEffectOp.GiveAllHandAndStartRecipientPindian, ResultBind: { } bind }, { Op: SkillProgramEffectOp.UsePindianWinnerSlash, SourceBind: { } read }] || bind != r.ResultBind || read != bind)
            return false;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RecipientContestGiftPaidEvent>().Count(e => e == new RecipientContestGiftPaidEvent(f.Id, r.Source, r.GameplayHash, r.TurnNumber, r.TurnOwnerSeat, recipient, bind, r.GiftCardIds.Count, r.SequenceBefore, r.SequenceAfter)) != 1 || r.GiftCardIds.Any(id => _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.CardId == id && m.From == CardLocation.Hand(f.OwnerSeat) && m.To == CardLocation.Hand(recipient) && m.Reason.Value == CoreGameEngineRecipientContestsRuntime.RecipientContestGiftReason(f)) != 1) || _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter) != r.GiftCardIds.Count)
            return false;
        if (r.Stage == RecipientContestStage.GiftChildren)
            return f.PendingMovementContinuation is { BeforeCount: 0, CoverageResultBind: null } pending && pending.SubjectSeat == f.OwnerSeat && r.ThirdSeat is null && r.PindianFrameId is null && r.SlashReturn is null;
        if (f.PendingMovementContinuation is not null)
            return false;
        if (r.Stage == RecipientContestStage.ChoosingThird)
        {
            if (r.ThirdSeat is not null || r.PindianFrameId is not null || r.SlashReturn is not null)
                return false;
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id)
                return true;
            var thirds = _coreRecipientContestsOperations.Value.RecipientContestThirds(f);
            return _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } prompt && prompt.PlayerSeat == f.OwnerSeat && prompt.TargetSeat == f.OwnerSeat && prompt.SkillPrompt?.SkillId == f.SkillId && prompt.ValidCardIds.Count == 0 && prompt.ValidTargetSeats.SequenceEqual(thirds) && prompt.Choices.Count == thirds.Length && prompt.Choices.All(c => c.Cards.Count == 0 && c.Targets is [var target] && thirds.Contains(target) && c.Id.Value == $"recipient-contest.{(f.Id)}.third-{(target)}" && c.Parameters.GetValueOrDefault("program-action") == "recipient-contest" && c.Parameters.GetValueOrDefault("frame-id") == f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        if (r.ThirdSeat is not { } third || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(third) || third == f.OwnerSeat || third == recipient || r.PindianFrameId is not { } pindianId || pindianId <= f.Id || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RecipientContestStartedEvent>().Count(e => e == new RecipientContestStartedEvent(f.Id, recipient, third, pindianId)) != 1)
            return false;
        if (r.Stage == RecipientContestStage.Pindian && _resolutionStackOperations._resolutionStack.OfType<PindianFrame>().SingleOrDefault(p => p.Id == pindianId)is { } contest)
            return contest.ParentFrameId == f.Id && contest.SourceSeat == recipient && contest.OpponentSeat == third && contest.ProgramResultBind == bind && contest.SkillId == f.SkillId;
        if (!_coreRecipientContestsOperations.Value.RecipientContestResultMatches(f, out var result))
            return false;
        if (r.SlashReturn is not { } returned)
            return r.Stage is RecipientContestStage.ResultReady or RecipientContestStage.Pindian or RecipientContestStage.Complete;
        if (result.SourceRank == result.OpponentRank || returned.ProgramFrameId != f.Id || returned.InstructionIndex != 2 || returned.Source != r.Source || returned.GameplayHash != f.GameplayHash || returned.ResultBind != bind || returned.PindianFrameId != pindianId || returned.RecipientSeat != recipient || returned.ThirdSeat != third || returned.RecipientRank != result.SourceRank || returned.ThirdRank != result.OpponentRank || returned.WinnerSeat != (result.SourceWon ? recipient : third) || returned.OriginalTargetSeat != (result.SourceWon ? third : recipient) || returned.CardUseFrameId <= pindianId || returned.CardActionId <= 0 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PindianWinnerSlashIssuedEvent>().Count(e => e.Return == returned) != 1)
            return false;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Id == returned.CardUseFrameId);
        return r.Stage == RecipientContestStage.SlashIssued ? use is not null && MatchesPindianWinnerUse(use, returned) : r.Stage == RecipientContestStage.Complete && use is null && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PindianWinnerSlashReturnedEvent>().Count(e => e.Return == returned) == 1;
    }

    internal bool MatchesPindianWinnerUse(CardUseFrame use, PindianWinnerSlashReturn returned)
    {
        if (use.PindianWinnerSlashReturn != returned || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || (use.CurrentSlashFirePolicy?.OriginalAction ?? use.Action)is not { Type: CardActionType.Use, EffectiveKind: CardKind.Slash } action || use.Action is not { Type: CardActionType.Use } currentAction || !currentAction.TargetSeats.SequenceEqual(use.TargetSeats) || use.CardKind != CardKind.Slash && !_coreCurrentSlashFireOffersOperations.IsCurrentSlashFireChangedUse(use) || action.ActionId != returned.CardActionId || action.ProviderSeat != returned.WinnerSeat || action.RequesterSeat is not null || action.ResponderSeat is not null || action.OpponentSeat is not null || action.PhysicalCards.Count != 0 || action.ConversionChain.Count != 0 || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseDeclaredEvent>().Any(e => e.ResolutionId == use.Id && e.CardId == 0 && e.CardKind == CardKind.Slash && e.SourceSeat == returned.WinnerSeat) || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TargetsConfirmedEvent>().Any(e => e.ResolutionId == use.Id && e.TargetSeats.SequenceEqual([returned.OriginalTargetSeat])))
            return false;
        if (use.CurrentSlashFirePolicy is not null)
            _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
        return (action.ActorSeat == returned.WinnerSeat && use.SourceSeat == returned.WinnerSeat || _coreActualTurnDamageClaimsOperations.MatchesDeclaredActualDamageUse(use, action.ActorSeat, returned.WinnerSeat)) && (use.TargetSeats.Count == 1 || ((_characterQianZhaoOlOperations.HasWeifuTargetTail(use) || ((_characterYangFuOlOperations.HasBingzhongTargetTail(use) || _characterMaChengOlOperations.HasChenglieTargetTail(use)) || _characterLiuPanOlOperations.HasPijingTargetTail(use))) || _characterMaWanOlOperations.HasMaWanHunjiangTargetTail(use) || (_characterZhangYanOlOperations.HasZhangYanLangdaoTargetTail(use) || _characterMaXiuMaTieOlOperations.HasKenshangTargetTail(use))) || _coreShortRangeSlashTargetsOperations.HasShortRangeSlashTail(use) || _coreSameTypeActualUseAidOperations.HasSameTypeAidTargetTail(use) || _coreOriginalTargetAdditionsOperations.HasIssuedOriginalTargetAdditionTail(use) || _coreCurrentSlashFireOffersOperations.IsCurrentSlashFireChangedUse(use));
    }
}
