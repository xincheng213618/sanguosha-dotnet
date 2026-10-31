#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoPiMouRuntime
{
    internal IReadOnlyList<MouCaoPiControl>? MouCaoPiControlsSnapshot(int seat)
    {
        var controls = ActiveMouCaoPiControls(seat).ToList();
        foreach (var grant in _coreMatchParticipantsPlayersAccess._players[seat].SkillGrants.Grants)
            if (grant.SkillState is { Acquisition: false, Action: SkillStateAction.Suppress, Scope: SkillStateScope.All,
                Duration: SkillStateDuration.UntilTurnEndOf, Origin: { } origin } state && origin.Source.SkillId == "mou:fangzhu")
                controls.Add(new(origin.FrameId, origin.Source.OwnerSeat, seat, state.IssuedTurn, MouCaoPiControlKind.SkillsInvalid));
        return controls.Count == 0 ? null : Array.AsReadOnly(controls.OrderBy(c => c.FrameId).ToArray());
    }

    internal IEnumerable<MouCaoPiControl> ActiveMouCaoPiControls(int seat)
    {
        return _characterCaoPiMouMouCaoPiControlsAccess._mouCaoPiControls.Where(c => c.TargetSeat == seat);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 28000)]
    internal void ExpireMouCaoPiControls(TurnEndedEvent e)
    {
        foreach (var c in ActiveMouCaoPiControls(e.ActorSeat).Where(c => e.TurnNumber > c.GrantedTurn).ToArray())
        {
            _characterCaoPiMouMouCaoPiControlsAccess._mouCaoPiControls.Remove(c);
            _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess._currentTurnSkillSuppressionRevision++;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouCaoPiControlExpiredEvent(c.FrameId, c.TargetSeat, e.TurnNumber));
        }
    }

    internal bool MouCaoPiCannotRespondTo(int responder, int source) => responder != source && ActiveMouCaoPiControls(responder).Any(c => c.Kind == MouCaoPiControlKind.CannotRespond);
    internal int? MouCaoPiResponseSource() => _resolutionStackOperations._resolutionStack.OfType<NullificationWindowFrame>().LastOrDefault()is { } counter ? counter.ChainDepth == 0 ? counter.SourceSeat : _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardActionAcceptedEvent>().LastOrDefault(e => e.Action.EffectiveKind == CardKind.Nullification)?.Action.ActorSeat ?? counter.SourceSeat : _coreActiveNativeAttackQueriesOperations.ActiveCardAttack?.CardUserSeat ?? _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.SourceSeat;
    internal bool MouCaoPiResponseForbidden(int actor, CardKind kind, CardActionType type) => (type == CardActionType.Response || kind == CardKind.Nullification) && MouCaoPiResponseSource()is { } source && MouCaoPiCannotRespondTo(actor, source);
    internal bool MouCaoPiHandUseForbidden(int actor, CardKind effectiveKind, IEnumerable<int> ids, bool response = false)
    {
        if (response && effectiveKind != CardKind.Nullification && !_coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[actor], effectiveKind))
            return false;
        if (!ids.Any(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(actor)))
            return false;
        var category = CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(effectiveKind);
        return ActiveMouCaoPiControls(actor).Any(c => c.Kind switch
        {
            MouCaoPiControlKind.BasicHandOnly => category != SkillProgramCardCategory.Basic,
            MouCaoPiControlKind.TrickHandOnly => category != SkillProgramCardCategory.Trick,
            MouCaoPiControlKind.EquipmentHandOnly => category != SkillProgramCardCategory.Equipment,
            _ => false
        });
    }

    [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 40)]
    internal bool MouCaoPiCardUseForbidden(ProgramCardUseQuery query) => MouCaoPiResponseForbidden(query.ActorSeat, query.Kind, query.ActionType);
    [ProgramCardLegality(ProgramCardLegalityStage.AfterLegalActionKind, 20)]
    internal bool MouCaoPiLegalActionForbidden(ProgramLegalActionQuery query) => query.Action.Kind != LegalActionKind.Recast && MouCaoPiHandUseForbidden(query.Actor.Seat, query.EffectiveKind!.Value, query.Action.CardId is { } material && material != 0 ? [material] : []);
    [ProgramCardLegality(ProgramCardLegalityStage.MaterialUseBan, 10)]
    internal bool MouCaoPiMaterialUseForbidden(ProgramCardMaterialQuery query) => MouCaoPiHandUseForbidden(query.ActorSeat, query.Kind, query.CardIds, query.IsResponse);
}
