#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramTieredRoundConversionRuntime
{
    internal bool TracksTrueRoundCardNames => _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesTieredRoundConversions || _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesLeiluanBasicUse;

    internal static bool IsTieredRoundOutput(CardKind kind) => CardCatalog.Get(kind).CategoryName == "基本牌" || global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind);
    internal int TieredRoundTier(int ownerSeat, string stateId) => _coreGameEngineConfiguredConversionsConfiguredConversionTiersAccess._configuredConversionTiers.GetValueOrDefault((ownerSeat, stateId));
    internal static string TieredRoundUsage(ProgramTieredRoundConversionPolicy policy) => "tiered-round-conversion:" + policy.StateId + ":" + policy.UsageId;
    internal bool TrueRoundHasUsedName(CardKind kind) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TrueRoundCardNameUsedEvent>().Any(e => e.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber && global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(e.EffectiveKind) == global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(kind));
    internal bool CanUseTieredRoundConversion(CharacterState actor, IndexedSkillProgramInstance instance, SkillProgramViewAs rule, bool responseUse, bool dyingUse)
    {
        if (_characterWoLongFengChuOlOperations.TryGetEquipmentSlotNamedConversionAvailability(actor, instance, rule, responseUse, dyingUse, out var equipmentAvailable))
            return equipmentAvailable;
        if (rule.TieredRoundConversion is not { } policy)
            return true;
        var tier = TieredRoundTier(actor.Seat, policy.StateId);
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !actor.IsAlive || _coreActualRoundProgressRoundNumberAccess._roundNumber < 1 || !IsTieredRoundOutput(rule.OutputKind) || tier is < 0 or > 2 || tier < rule.MinimumTier || tier > rule.MaximumTier || rule.ConversionStateId != policy.StateId || rule.InputCount != (tier == 2 ? 0 : 1))
            return false;
        // Response grammar is admitted only for a real use-qualified conversion;
        // ordinary Slash/Dodge response paths never become round-use facts.
        if (responseUse && !rule.UseOnly)
            return false;
        if (tier == 0 && (_coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || actor.Seat != _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat || TrueRoundHasUsedName(rule.OutputKind)))
            return false;
        var scope = tier == 0 ? SkillUsageScope.Phase : SkillUsageScope.Round;
        return _skillRuntimeStateOperations._skillRuntimeState.GetUsage(actor.Seat, instance.SkillId, TieredRoundUsage(policy), scope) == 0;
    }

    internal void ConsumeTieredRoundConversion(CardConversionSource source)
    {
        if (_characterWoLongFengChuOlOperations.TryConsumePaidEquipmentSlotNamedConversion(source))
            return;
        if (_corePlayCardHistoryOperations.ViewAsRule(source)is not { TieredRoundConversion: { } policy } rule)
            return;
        var actor = _coreMatchParticipantsPlayersAccess._players[source.OwnerSeat];
        var instance = _coreNationalSkillsOperations.GetSkillBindingShard(actor).ProgramInstances.SingleOrDefault(i => i.SkillId == source.SkillId && i.SkillInstanceId == source.SkillInstanceId);
        if (instance is null || !CanUseTieredRoundConversion(actor, instance, rule, false, false))
            throw new InvalidOperationException("A tiered round conversion lost its published source, current level or shared allowance before acceptance.");
        var tier = TieredRoundTier(actor.Seat, policy.StateId);
        var scope = tier == 0 ? SkillUsageScope.Phase : SkillUsageScope.Round;
        if (!_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(actor.Seat, source.SkillId, TieredRoundUsage(policy), scope, 1))
            throw new InvalidOperationException("A tiered conversion allowance cannot be paid twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillUsageConsumedEvent(actor.Seat, source.SkillId, source.BindingId, scope, 1));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 320)]
    internal void ObserveTrueRoundCardNames(IGameEvent payload)
    {
        if (!TracksTrueRoundCardNames || _coreActualRoundProgressRoundNumberAccess._roundNumber < 1)
            return;
        if (payload is CardActionAcceptedEvent accepted)
        {
            var acceptedAction = accepted.Action;
            if (TryRecordTrueRoundDodgeUse(acceptedAction))
                return;
            if (acceptedAction.Type != CardActionType.Response || acceptedAction.EffectiveKind != CardKind.Nullification || acceptedAction.ActorSeat != acceptedAction.ProviderSeat || acceptedAction.ResponderSeat != acceptedAction.ActorSeat || acceptedAction.RequesterSeat is not null)
                return;
            var window = _resolutionStackOperations._resolutionStack.OfType<NullificationWindowFrame>().LastOrDefault();
            var parent = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Id == window?.ParentFrameId);
            if (window is null || parent is null || acceptedAction.ParentActionId != parent.Action?.ActionId || acceptedAction.OpponentSeat != window.SourceSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(acceptedAction.ActorSeat))
                return;
            RecordTrueRoundCardName(window.Id, acceptedAction.ActorSeat, acceptedAction.ActionId, acceptedAction.EffectiveKind, true);
            return;
        }

        if (payload is not CardUseDeclaredEvent declared)
            return;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Id == declared.ResolutionId);
        if (use is null || use.SourceSeat != declared.SourceSeat || use.CardId != declared.CardId || use.CardKind != declared.CardKind || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(use.SourceSeat) || !IsTieredRoundOutput(use.CardKind))
            return;
        if (use.Action is { } action)
        {
            if (action.Type != CardActionType.Use || action.ActorSeat != use.SourceSeat || action.EffectiveKind != use.CardKind)
                return;
            // A declaration reaches this point only after the real producer
            // accepted it and transferred its frozen declaration payment.
            if (use.AcceptedDeclarationPayment is { } payment && (payment.OwnerFrameId != use.Id || payment.DeclaredKind != use.CardKind || payment.ProviderSeat != action.ProviderSeat))
                throw new InvalidOperationException("Round-use tracking lost the accepted declared-use producer.");
            RecordTrueRoundCardName(use.Id, action.ActorSeat, action.ActionId, use.CardKind, false);
        }
        else if (use.CardId == 0 && use.PhysicalCardIds is { Count: 0 } && _coreCurrentTurnUseKindsOperations.IsActualTurnLegacyVirtualUse(use))
            RecordTrueRoundCardName(use.Id, use.SourceSeat, null, use.CardKind, false);
    }

    internal void RecordTrueRoundCardName(long origin, int actor, long? action, CardKind kind, bool counter)
    {
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TrueRoundCardNameUsedEvent>().Any(e => e.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber && (action is not null ? e.CardActionId == action : e.CardActionId is null && e.OriginFrameId == origin)))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TrueRoundCardNameUsedEvent(_coreActualRoundProgressRoundNumberAccess._roundNumber, actor, origin, action, kind, counter));
    }

    internal bool TryRecordTrueRoundDodgeUse(CardActionContext action)
    {
        if (action.Type != CardActionType.Response || action.EffectiveKind != CardKind.Dodge || action.ActorSeat != action.ProviderSeat || action.ResponderSeat != action.ActorSeat || action.RequesterSeat is not null || _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is not null || _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is not null || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not { } attack || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash) || attack.TargetSeat != action.ActorSeat || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { } use || action.ParentActionId != use.Action?.ActionId || action.OpponentSeat != attack.SourceSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(action.ActorSeat) || !_coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], CardKind.Dodge))
            return false;
        // This observer runs at the real Accepted boundary. Native, converted
        // and Bagua Dodge uses share the same mature Slash-defense classification;
        // a new zero-material conversion additionally retains its own issuance.
        if (action.PhysicalCards.Count == 0 && action.ConversionChain.Any(source => _corePlayCardHistoryOperations.ViewAsRule(source)is { InputCount: 0, TieredRoundConversion: not null }) && !_coreTieredRoundZeroResponsesOperations.IsIssuedTieredRoundZeroResponse(use, action))
            return false;
        RecordTrueRoundCardName(use.Id, action.ActorSeat, action.ActionId, CardKind.Dodge, false);
        return true;
    }

    [ProgramCardLegality(ProgramCardLegalityStage.ConfiguredConversionBan, 10)]
    internal bool TieredRoundConversionUnavailable(ProgramConfiguredViewAsQuery query) => !CanUseTieredRoundConversion(query.Owner, query.Instance, query.Rule, query.ForResponse, query.DyingUse);
}
