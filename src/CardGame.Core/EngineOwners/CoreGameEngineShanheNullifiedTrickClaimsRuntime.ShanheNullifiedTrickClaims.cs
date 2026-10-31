#nullable enable

using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheNullifiedTrickClaimsRuntime
{
    [ProgramPipelineHook(ProgramPipelinePhase.BeforeNullificationFinished, 80900)]
    internal bool TryBeginShanheNullifiedTrickClaim(NullificationWindowFrame window)
    {
        if (_coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is null || _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id != ShanheBattleSetup.BattleModeId)
            return false;
        if (window.DelayedReturn is not null)
            return _coreShanheDelayedNullifiedTrickClaimsOperations.TryBeginShanheDelayedNullifiedTrickClaim(window);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { Action: { Type: CardActionType.Use } action } use)
            return false;
        if (use.ShanheNullifiedTrickClaim is { } existing)
        {
            if (existing.WindowFrameId != window.Id || existing.MovementDrained)
                return false;
            if (_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(window.Id))
                return true;
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { ShanheNullifiedTrickClaim = existing with { MovementDrained = true } });
            return false;
        }

        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !window.EffectNullified || window.ChainDepth <= 0 || _coreCurrentCardEnhancementsOperations.HasCurrentCardEnhancement(use.Id, CurrentCardEnhancement.Uncancelable) || window.DelayedReturn is not null)
            return false;
        var response = _coreProgramEventHistoryOperations.ProgramEventHistory<NullificationRespondedEvent>().LastOrDefault(fact => fact.ResolutionId == use.Id && fact.WindowFrameId == window.Id && fact.ChainDepth == window.ChainDepth && fact.EffectNullified && fact.EffectCardId == window.EffectCardId && fact.EffectCardKind == window.EffectCardKind);
        if (response is null || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(response.ResponderSeat) || !_coreMatchParticipantsPlayersAccess._players[response.ResponderSeat].IsAlive)
            return false;
        var binding = _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[response.ResponderSeat], SkillProgramCardPolicyKind.ClaimNullifiedTrick, CardKind.Nullification).FirstOrDefault();
        if (binding.Source is null)
            return false;
        var ids = (use.PhysicalCardIds ?? [use.CardId]).Where(id => id > 0 && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing && !_coreCurrentUsePhysicalClaimsOperations.IsClaimedUseCardEntity(use.Id, id)).ToArray();
        if (ids.Length == 0)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != window.Id || action.ActorSeat != window.SourceSeat || ids.Distinct().Count() != ids.Length || ids.Any(id => !action.PhysicalCards.Any(cost => cost.CardId == id)))
            throw new InvalidOperationException("A Shanhe counterspell claim lost its exact final window and original paid materials.");
        var receipt = new ShanheNullifiedTrickClaim(use.Id, action.ActionId, window.Id, window.ChainDepth, CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(window), response.ResponderSeat, response.NullificationCardId, binding.Source.SkillId, binding.Source.SkillInstanceId, binding.Source.Program.GameplayHash, binding.Policy.Id, Array.AsReadOnly(ids), _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        // The still-resolving use owns the entities before any movement observers
        // can inspect the parent. No later target or completion can claim again.
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { ShanheNullifiedTrickClaim = receipt });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(receipt.RecipientSeat), CoreGameEngineShanheNullifiedTrickClaimsDefinitions.ShanheCounterspellClaimReason);
        receipt = receipt with
        {
            MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { ShanheNullifiedTrickClaim = receipt });
        foreach (var id in ids)
        {
            var move = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(item => item.CardId == id && item.Sequence > receipt.MovementBefore && item.Sequence <= receipt.MovementAfter && item.From == CardLocation.Processing && item.To == CardLocation.Hand(receipt.RecipientSeat) && item.Reason == CoreGameEngineShanheNullifiedTrickClaimsDefinitions.ShanheCounterspellClaimReason);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheNullifiedTrickMaterialClaimedEvent(use.Id, action.ActionId, window.Id, window.ChainDepth, receipt.RecipientSeat, id, move.Sequence));
        }

        return TryBeginShanheNullifiedTrickClaim(window);
    }

    internal bool IsShanheNullifiedTrickMaterialClaim(long useId, int cardId) => _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { Action: { } action, ShanheNullifiedTrickClaim: { } receipt } && receipt.CardUseFrameId == useId && receipt.ActionId == action.ActionId && receipt.CardIds.Contains(cardId);
    internal bool IsShanheNullifiedClaimAwaitingMovement(NullificationWindowFrame window) => _coreShanheDelayedNullifiedTrickClaimsOperations.IsShanheDelayedNullifiedClaimAwaitingMovement(window) || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)?.ShanheNullifiedTrickClaim is { MovementDrained: false } receipt && receipt.CardUseFrameId == window.ParentFrameId && receipt.WindowFrameId == window.Id;
    internal bool IsShanheNullifiedClaimMovement(NullificationWindowFrame window, CardMovementBatchContext batch) => _coreShanheDelayedNullifiedTrickClaimsOperations.IsShanheDelayedNullifiedClaimMovement(window, batch) || IsShanheNullifiedClaimAwaitingMovement(window) && batch.ParentFrameId == window.Id && batch.AwaitingProgramFrameId is null && batch.Movements.Count > 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)?.ShanheNullifiedTrickClaim is { } receipt && batch.Movements.All(move => move.Sequence > receipt.MovementBefore && move.Sequence <= receipt.MovementAfter && receipt.CardIds.Contains(move.CardId) && move.From == CardLocation.Processing && move.To == CardLocation.Hand(receipt.RecipientSeat) && move.Reason == CoreGameEngineShanheNullifiedTrickClaimsDefinitions.ShanheCounterspellClaimReason);
    internal static bool HasUnlistedShanheClaimMovementReturn(CardsMovedTriggerWindowFrame child) => child.ResumeColdDamageReplacementFrameId is not null || child.ResumeSkillInvocationFrameId is not null || child.ResumeRoundEndingBoundaryId is not null || child.ResumeMouZhitianFireFrameId is not null || child.QiaoZhouPreparationReturn is not null || child.SoleTargetQuarantineEndReturn is not null || child.WenhePoisonFireAttackReturn is not null;
    internal static bool HasOnlyShanheNullifiedClaimMovementReturn(CardsMovedTriggerWindowFrame child) => child.ResumeShanheNullifiedClaimWindowId is not null && child.Id == child.Batch.Id && child.ResumeProgramFrameId is null && !CoreDamageObserverCursorQueriesRuntime.HasAlternateProgramMovementReturn(child with { ResumeShanheNullifiedClaimWindowId = null }) && !HasUnlistedShanheClaimMovementReturn(child);
    internal void ReturnShanheNullifiedClaimMovement(CardsMovedTriggerWindowFrame child)
    {
        if (!HasOnlyShanheNullifiedClaimMovementReturn(child) || child.ResumeShanheNullifiedClaimWindowId is not { } id || _resolutionStackOperations._resolutionStack.LastOrDefault()is not NullificationWindowFrame window || window.Id != id || !IsShanheNullifiedClaimMovement(window, child.Batch))
            throw new InvalidOperationException("A Shanhe counterspell claim lost its original awaited movement return.");
        _coreNativeNullificationOperations.FinishNullificationWindow(window);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 80900)]
    internal void AssertShanheNullifiedTrickClaims()
    {
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(frame => frame.ShanheNullifiedTrickClaim is not null))
        {
            var receipt = use.ShanheNullifiedTrickClaim!;
            if (_coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is null || _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id != ShanheBattleSetup.BattleModeId || use.Action is not { Type: CardActionType.Use } action || receipt.CardUseFrameId != use.Id || receipt.ActionId != action.ActionId || receipt.WindowFrameId <= use.Id || receipt.ChainDepth <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(receipt.RecipientSeat) || receipt.CardIds.Count == 0 || receipt.CardIds.Distinct().Count() != receipt.CardIds.Count || receipt.CardIds.Any(id => !action.PhysicalCards.Any(cost => cost.CardId == id)) || receipt.MovementAfter <= receipt.MovementBefore || string.IsNullOrWhiteSpace(receipt.SkillInstanceId) || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(receipt.SkillId)?.Program is not { } program || program.GameplayHash != receipt.GameplayHash || !program.CardPolicies.Any(policy => policy.Id == receipt.PolicyId && policy.Kind == SkillProgramCardPolicyKind.ClaimNullifiedTrick) || !_coreProgramEventHistoryOperations.ProgramEventHistory<NullificationRespondedEvent>().Any(fact => fact.WindowFrameId == receipt.WindowFrameId && fact.ResolutionId == use.Id && fact.ChainDepth == receipt.ChainDepth && fact.EffectNullified && fact.ResponderSeat == receipt.RecipientSeat && fact.NullificationCardId == receipt.ResponseCardId) || receipt.CardIds.Any(id => _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheNullifiedTrickMaterialClaimedEvent>().Count(fact => fact.CardUseFrameId == use.Id && fact.ActionId == receipt.ActionId && fact.WindowFrameId == receipt.WindowFrameId && fact.ChainDepth == receipt.ChainDepth && fact.RecipientSeat == receipt.RecipientSeat && fact.CardId == id && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.Sequence == fact.MovementSequence && move.Sequence > receipt.MovementBefore && move.Sequence <= receipt.MovementAfter && move.CardId == id && move.From == CardLocation.Processing && move.To == CardLocation.Hand(receipt.RecipientSeat) && move.Reason == CoreGameEngineShanheNullifiedTrickClaimsDefinitions.ShanheCounterspellClaimReason)) != 1))
                throw new InvalidOperationException("A Shanhe material claim lost its original paid use, final counterspell or exact one-time movement proof.");
        }
    }
}
