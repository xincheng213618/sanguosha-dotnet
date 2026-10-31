#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWushuangSkillsBAuditRuntime
{
    internal const string BJiushi = "wushuang:skill-3269", BGuzheng = "wushuang:skill-3139", BFuqi = "wushuang:skill-3100", BDuwu = "wushuang:skill-3062", BLonghun = "wushuang:skill-3023";
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 61201)]
    internal void ObserveWushuangBAudit(IGameEvent payload)
    {
        if (payload is DamageRequestedEvent request && _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[request.TargetSeat]).HasSkill(BJiushi))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangJiushiDamageFaceEvent(request.ResolutionId, request.TargetSeat, _coreMatchParticipantsPlayersAccess._players[request.TargetSeat].IsFaceDown));
        if (payload is PhaseChangedEvent { Phase: TurnPhase.Discard } phase && _contentRegistryOperations._contentRegistry.Skills.ContainsKey(BGuzheng))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangDiscardPhaseBoundaryEvent(_coreActualTurnProgressTurnNumberAccess._turnNumber, phase.ActorSeat, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 61201)]
    internal bool CanTriggerWushuangBAudit(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (c.SkillId == BJiushi && trigger.Id == "flip-back-after-damage")
            return _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsFaceDown && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangJiushiDamageFaceEvent>().Any(e => e.OwnerSeat == c.OwnerSeat && e.DamageFrameId == context.DamageFrameId && e.WasFaceDown);
        if (c.SkillId == BLonghun)
            return context.CardUse is { } use && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangCardUseNameRecordedEvent>().Any(e => e.CardActionId == use.CardActionId);
        if (c.SkillId == BGuzheng)
            return context.SourceSeat is { } seat && WushuangGuzhengCards(seat).Count > 0;
        return true;
    }

    internal bool WushuangFuqiCannotRespond(int source, int responder) => source != responder && _coreTableSeatQueriesOperations.IsValidPlayerSeat(source) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(responder) && _coreMatchParticipantsPlayersAccess._players[source].IsAlive && _coreMatchParticipantsPlayersAccess._players[responder].IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[source]).HasSkill(BFuqi) && _coreNativeRuleQueriesOperations.GetCombatDistance(source, responder) == 1;
    internal bool WushuangFuqiNullificationBanned(int responder)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not { } window)
            return false;
        var source = window.ChainDepth == 0 ? window.SourceSeat : _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NullificationRespondedEvent>().LastOrDefault(e => e.ResolutionId == window.ParentFrameId && e.ChainDepth == window.ChainDepth)?.ResponderSeat ?? -1;
        return WushuangFuqiCannotRespond(source, responder);
    }

    [ProgramCardLegality(ProgramCardLegalityStage.NativeResponseBan, 61201)]
    internal bool WushuangFuqiNativeResponseForbidden(ProgramNativeResponseQuery query) => !query.DyingUse && query.Kind == CardKind.Nullification && WushuangFuqiNullificationBanned(query.Provider.Seat);
    internal IReadOnlyList<int> WushuangGuzhengCards(int phaseOwner)
    {
        var boundary = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangDiscardPhaseBoundaryEvent>().LastOrDefault(e => e.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber && e.ActorSeat == phaseOwner);
        if (boundary is null)
            return[];
        return _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > boundary.MovementSequence && m.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber && m.To == CardLocation.DiscardPile && _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(m)is not null && _cardZonesOperations._cardZones.GetLocation(m.CardId) == CardLocation.DiscardPile).Select(m => m.CardId).Distinct().Order().ToArray();
    }

    internal void AddWushuangGuzhengChoices(ProgramSkillFrame f, ICollection<PromptChoice> choices)
    {
        var r = f.WushuangB!;
        if (r.Stage == "guzheng-return")
            foreach (var id in WushuangGuzhengCards(r.TargetSeat))
                choices.Add(_coreWushuangSkillsBOperations.BChoice(f, r.Stage, id.ToString(System.Globalization.CultureInfo.InvariantCulture), $"将【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)}】交还给{(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].Name)}", [id]));
        if (r.Stage == "guzheng-claim")
        {
            choices.Add(_coreWushuangSkillsBOperations.BChoice(f, r.Stage, "claim", "获得剩余弃置牌"));
            choices.Add(_coreWushuangSkillsBOperations.BChoice(f, r.Stage, "pass", "不获得剩余弃置牌"));
        }
    }

    internal bool AnswerWushuangGuzheng(ProgramSkillFrame f, PromptChoice choice)
    {
        var r = f.WushuangB!;
        if (r.Stage is not ("guzheng-return" or "guzheng-claim"))
            return false;
        if (r.Stage == "guzheng-return")
        {
            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards.Single());
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangB = r with { Stage = "guzheng-returned", Cards = WushuangGuzhengCards(r.TargetSeat).Where(id => id != card.Id).ToArray() } });
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DiscardPile, CardLocation.Hand(r.TargetSeat), new("skill-program.wushuang-guzheng.return"));
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangB = r with { Stage = "complete" } });
        if (choice.Parameters["value"] == "claim")
            foreach (var id in r.Cards.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.DiscardPile))
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.DiscardPile, CardLocation.Hand(f.OwnerSeat), new("skill-program.wushuang-guzheng.claim"));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }
}
