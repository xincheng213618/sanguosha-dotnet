#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWushuangSkillsBAuditRuntime
{
    internal const string BJiushi = "wushuang:skill-3269", BFuqi = "wushuang:skill-3100", BDuwu = "wushuang:skill-3062", BLonghun = "wushuang:skill-3023";
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 61201)]
    internal void ObserveWushuangBAudit(IGameEvent payload)
    {
        if (payload is DamageRequestedEvent request && _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[request.TargetSeat]).HasSkill(BJiushi))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangJiushiDamageFaceEvent(request.ResolutionId, request.TargetSeat, _coreMatchParticipantsPlayersAccess._players[request.TargetSeat].IsFaceDown));
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 61201)]
    internal bool CanTriggerWushuangBAudit(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (c.SkillId == BJiushi && trigger.Id == "flip-back-after-damage")
            return _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsFaceDown && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangJiushiDamageFaceEvent>().Any(e => e.OwnerSeat == c.OwnerSeat && e.DamageFrameId == context.DamageFrameId && e.WasFaceDown);
        if (c.SkillId == BLonghun)
            return context.CardUse is { } use && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WushuangCardUseNameRecordedEvent>().Any(e => e.CardActionId == use.CardActionId);
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
}
