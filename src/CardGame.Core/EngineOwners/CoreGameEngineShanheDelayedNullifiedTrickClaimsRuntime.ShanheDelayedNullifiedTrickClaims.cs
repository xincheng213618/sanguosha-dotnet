#nullable enable

using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheDelayedNullifiedTrickClaimsRuntime
{
    internal bool TryBeginShanheDelayedNullifiedTrickClaim(NullificationWindowFrame window)
    {
        if (window.DelayedReturn is not { OriginalUseLightning: null, CardId: > 0 } delayed)
            return false;
        if (window.ShanheDelayedNullifiedTrickClaim is { } existing)
        {
            if (existing.MovementDrained)
                return false;
            if (_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(window.Id))
                return true;
            _coreRuntimeOperations.ReplaceRuntimeFrame(window.Id, window with { ShanheDelayedNullifiedTrickClaim = existing with { MovementDrained = true } });
            return false;
        }

        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !window.EffectNullified || window.ChainDepth <= 0 || !_coreDelayedNullificationOperations.Value.HasNullificationEffectOwner(window))
            return false;
        var response = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NullificationRespondedEvent>().LastOrDefault(fact => fact.WindowFrameId == window.Id && fact.ResolutionId == window.ParentFrameId && fact.EffectCardId == delayed.CardId && fact.EffectCardKind == delayed.EffectiveKind && fact.ChainDepth == window.ChainDepth && fact.EffectNullified);
        if (response is null || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(response.ResponderSeat) || !_coreMatchParticipantsPlayersAccess._players[response.ResponderSeat].IsAlive)
            return false;
        var binding = _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[response.ResponderSeat], SkillProgramCardPolicyKind.ClaimNullifiedTrick, CardKind.Nullification).FirstOrDefault();
        if (binding.Source is null)
            return false;
        var card = _corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[delayed.OwnerSeat]).Single(value => value.Id == delayed.CardId);
        var receipt = new ShanheDelayedNullifiedTrickClaim(window.Id, delayed, window.ChainDepth, response.ResponderSeat, response.NullificationCardId, binding.Source.SkillId, binding.Source.SkillInstanceId, binding.Source.Program.GameplayHash, binding.Policy.Id, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != window.Id)
            throw new InvalidOperationException("A delayed material claim must remain on its original live counterspell window.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(window.Id, window = window with { ShanheDelayedNullifiedTrickClaim = receipt });
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Judgment(delayed.OwnerSeat), CardLocation.Hand(receipt.RecipientSeat), CoreGameEngineShanheNullifiedTrickClaimsDefinitions.ShanheCounterspellClaimReason);
        receipt = receipt with
        {
            MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(window.Id, window = window with { ShanheDelayedNullifiedTrickClaim = receipt });
        var move = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(value => value.CardId == delayed.CardId && value.Sequence > receipt.MovementBefore && value.Sequence <= receipt.MovementAfter && value.From == CardLocation.Judgment(delayed.OwnerSeat) && value.To == CardLocation.Hand(receipt.RecipientSeat) && value.Reason == CoreGameEngineShanheNullifiedTrickClaimsDefinitions.ShanheCounterspellClaimReason);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheDelayedNullifiedTrickMaterialClaimedEvent(window.Id, delayed.CardId, delayed.EffectiveKind, delayed.OwnerSeat, receipt.RecipientSeat, window.ChainDepth, move.Sequence));
        return TryBeginShanheDelayedNullifiedTrickClaim(window);
    }

    internal bool IsShanheDelayedNullifiedClaimAwaitingMovement(NullificationWindowFrame window) => window.ShanheDelayedNullifiedTrickClaim is { MovementDrained: false } receipt && receipt.WindowFrameId == window.Id && receipt.Origin == window.DelayedReturn;
    internal bool IsShanheDelayedNullifiedClaimMovement(NullificationWindowFrame window, CardMovementBatchContext batch) => IsShanheDelayedNullifiedClaimAwaitingMovement(window) && batch.ParentFrameId == window.Id && batch.AwaitingProgramFrameId is null && batch.Movements.Count == 1 && window.ShanheDelayedNullifiedTrickClaim is { } receipt && batch.Movements[0] is { } move && move.Sequence > receipt.MovementBefore && move.Sequence <= receipt.MovementAfter && move.CardId == receipt.Origin.CardId && move.From == CardLocation.Judgment(receipt.Origin.OwnerSeat) && move.To == CardLocation.Hand(receipt.RecipientSeat) && move.Reason == CoreGameEngineShanheNullifiedTrickClaimsDefinitions.ShanheCounterspellClaimReason;
    internal bool HasShanheClaimedDelayedOwner(NullificationWindowFrame window, DelayedEffectContinuation delayed)
    {
        if (window.ShanheDelayedNullifiedTrickClaim is not { } receipt)
            return false;
        ValidateShanheDelayedNullifiedTrickClaim(window, receipt);
        return receipt.Origin == delayed;
    }

    internal bool TryCompleteShanheClaimedDelayedNullification(NullificationWindowFrame window, DelayedEffectContinuation delayed)
    {
        if (window.ShanheDelayedNullifiedTrickClaim is not { } receipt)
            return false;
        if (!receipt.MovementDrained || !HasShanheClaimedDelayedOwner(window, delayed))
            throw new InvalidOperationException("A claimed delayed effect cannot finish before its actual gain children return.");
        _coreMatchLogOperations.AddLog("CardEffect", $"【{(CardCatalog.Get(delayed.EffectiveKind).DisplayName)}】的效果被无懈。", delayed.OwnerSeat);
        if (delayed.EffectiveKind == CardKind.Lightning)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LightningResolvedEvent(window.Id, delayed.CardId, delayed.OwnerSeat, null, Hit: false, NextTargetSeat: null, DamageAmount: 0));
        if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[delayed.OwnerSeat].IsAlive)
            _coreActualTurnsOperations.BeginDelayedJudgmentOrTurnStart(_coreMatchParticipantsPlayersAccess._players[delayed.OwnerSeat]);
        return true;
    }

    internal void ValidateShanheDelayedNullifiedTrickClaim(NullificationWindowFrame window, ShanheDelayedNullifiedTrickClaim receipt)
    {
        var delayed = receipt.Origin;
        if (_coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is null || _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id != ShanheBattleSetup.BattleModeId || window.DelayedReturn != delayed || delayed.OriginalUseLightning is not null || delayed.CardId <= 0 || window.Id != receipt.WindowFrameId || window.ParentFrameId != delayed.ParentFrameId || window.EffectCardId != delayed.CardId || window.EffectCardKind != delayed.EffectiveKind || window.SourceSeat != delayed.OwnerSeat || !window.TargetSeats.SequenceEqual([delayed.OwnerSeat]) || !window.EffectNullified || receipt.ChainDepth != window.ChainDepth || receipt.ChainDepth <= 0 || !CoreNativeCardClassificationsRuntime.IsDelayedCard(delayed.EffectiveKind) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(receipt.RecipientSeat) || delayed.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || delayed.Phase != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase || delayed.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || receipt.MovementAfter <= receipt.MovementBefore || string.IsNullOrWhiteSpace(receipt.SkillInstanceId) || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(receipt.SkillId)?.Program is not { } program || program.GameplayHash != receipt.GameplayHash || !program.CardPolicies.Any(policy => policy.Id == receipt.PolicyId && policy.Kind == SkillProgramCardPolicyKind.ClaimNullifiedTrick) || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<NullificationRespondedEvent>().Any(fact => fact.WindowFrameId == window.Id && fact.ResolutionId == window.ParentFrameId && fact.EffectCardId == delayed.CardId && fact.EffectCardKind == delayed.EffectiveKind && fact.ChainDepth == receipt.ChainDepth && fact.EffectNullified && fact.ResponderSeat == receipt.RecipientSeat && fact.NullificationCardId == receipt.ResponseCardId) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShanheDelayedNullifiedTrickMaterialClaimedEvent>().Count(fact => fact.WindowFrameId == window.Id && fact.CardId == delayed.CardId && fact.EffectiveKind == delayed.EffectiveKind && fact.JudgmentOwnerSeat == delayed.OwnerSeat && fact.RecipientSeat == receipt.RecipientSeat && fact.ChainDepth == receipt.ChainDepth && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.Sequence == fact.MovementSequence && move.Sequence > receipt.MovementBefore && move.Sequence <= receipt.MovementAfter && move.CardId == delayed.CardId && move.From == CardLocation.Judgment(delayed.OwnerSeat) && move.To == CardLocation.Hand(receipt.RecipientSeat) && move.Reason == CoreGameEngineShanheNullifiedTrickClaimsDefinitions.ShanheCounterspellClaimReason)) != 1)
            throw new InvalidOperationException("A delayed claim lost its exact original window, delayed-return token or actual final-counterspell movement.");
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 80901)]
    internal void AssertShanheDelayedNullifiedTrickClaims()
    {
        foreach (var window in _resolutionStackOperations._resolutionStack.OfType<NullificationWindowFrame>())
            if (window.ShanheDelayedNullifiedTrickClaim is { } receipt)
                ValidateShanheDelayedNullifiedTrickClaim(window, receipt);
    }
}
