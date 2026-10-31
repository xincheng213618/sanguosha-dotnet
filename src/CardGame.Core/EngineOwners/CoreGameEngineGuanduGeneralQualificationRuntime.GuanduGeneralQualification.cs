#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGuanduGeneralQualificationRuntime
{
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeOwnerFacts, 44000)]
    internal bool AllowsGuanduGeneralActivation(ProgramActivationLegalityContext gate)
    {
        if (!gate.SkillId.StartsWith("guandu:", StringComparison.Ordinal))
            return true;
        if (!_coreGuanduGeneralsOperations.IsGuanduGeneralContextAllowed(gate.SkillId, SkillProgramEffectOp.ResolveGuanduMoushi))
            return false;
        if (gate.SkillId == "guandu:shicai")
            return _coreGuanduGeneralsOperations.GuanduShicaiReady(gate.OwnerSeat, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(gate.Owner, gate.SkillId)) && _cardZonesOperations._cardZones.Count(CardLocation.DrawPile) > 0 && _coreGuanduGeneralsOperations.GuanduDiscardable(gate.OwnerSeat, gate.OwnerSeat).Length > 0;
        if (gate.SkillId == "guandu:zezhu")
            return _coreMatchParticipantsPlayersAccess._players[gate.OwnerSeat].SkillGrants.Grants.Any(g => g.SkillId == gate.SkillId && _coreGuanduGeneralsOperations.GuanduGeneralGrantQualified(_coreMatchParticipantsPlayersAccess._players[gate.OwnerSeat], g));
        if (gate.SkillId == "guandu:yuanlve")
            return _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[gate.OwnerSeat]).Any(c => !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(c.Kind));
        if (gate.SkillId == "guandu:moushi")
            return _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[gate.OwnerSeat]).Count > 0;
        return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 44000)]
    internal bool AllowsGuanduGeneralTrigger(ProgramTriggerLegalityContext gate)
    {
        if (!gate.SkillId.StartsWith("guandu:", StringComparison.Ordinal))
            return true;
        if (!_coreGuanduGeneralsOperations.IsGuanduGeneralContextAllowed(gate.SkillId, SkillProgramEffectOp.ResolveGuanduMoushi))
            return false;
        var c = gate.Context;
        var owner = gate.Owner;
        switch (gate.SkillId)
        {
            case "guandu:chenggong":
                return c.CardUse is { } use && _coreCardUseLifecycleOperations.LifecycleCardUse(use.ParentCardUseFrameId)?.Action is { } action && action.EffectiveDesignatedTargetSeats.Distinct().Count() >= 2;
            case "guandu:jieliang":
                if (c.Window == SkillProgramTriggerWindow.DrawPhaseStarting)
                    return c.SourceSeat != owner.Seat && _coreGuanduGeneralsOperations.GuanduDiscardable(owner.Seat, owner.Seat).Length > 0;
                return c.SourceSeat is { } discarded && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<GuanduJieliangPaidEvent>().Any(e => e.OwnerSeat == owner.Seat && e.TargetSeat == discarded && e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber) && _coreGuanduGeneralBenefitsOperations.GuanduClaimableDiscards(discarded).Length > 0;
            case "guandu:cangchu":
                return c.Window == SkillProgramTriggerWindow.GameStarting || c.TargetSeat == owner.Seat && c.Amount > 0 && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } fire && _coreNativeCardClassificationsOperations.GetDamageNature(fire) == DamageNature.Fire;
            case "guandu:polu":
                return !_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Any(card => card.Kind == CardKind.ThunderChariot) && (c.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow || c.TargetSeat == owner.Seat && c.Amount > 0);
            case "guandu:thunder-chariot":
                return c.SourceSeat == owner.Seat && c.TargetSeat is { } victim && victim != owner.Seat && _coreMatchParticipantsPlayersAccess._players[victim].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Any(card => card.Kind == CardKind.ThunderChariot && _coreGuanduEquipmentOperations.GuanduEquipmentSkillGrantId(card) == gate.Candidate.SkillInstanceId) && _coreGuanduEquipmentOperations.GuanduChariotDiscardable(owner.Seat, victim).Length > 0;
            case "guandu:fuyuan":
                return _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber > 0 && _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat != owner.Seat && _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat].IsAlive;
            case "guandu:hengjiang":
                return c.Window == SkillProgramTriggerWindow.AfterDamageApplied ? c.Amount > 0 && _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber > 0 : _coreProgramEventHistoryOperations.Value.ProgramEventHistory<GuanduHengjiangPaidEvent>().Any(e => e.OwnerSeat == owner.Seat && e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber) && !_coreGuanduGeneralsOperations.GuanduTurnDiscarded(_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat);
            case "guandu:xiying":
                return _coreGuanduGeneralsOperations.GuanduDiscardable(owner.Seat, owner.Seat).Length > 0;
            case "guandu:choulue":
                return _coreGuanduGeneralNativeCardsOperations.GuanduLastDamageCard(owner.Seat)is not null && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != owner.Seat && _coreGuanduGeneralsOperations.GuanduOwnedCards(p.Seat).Length > 0);
            case "guandu:sijian":
                return c.MovementBatch is { } batch && batch.SourceCounts.Any(s => s.Location == CardLocation.Hand(owner.Seat) && s.CountBefore > 0 && s.CountAfter == 0) && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != owner.Seat && _coreGuanduGeneralsOperations.GuanduOwnedCards(p.Seat, true).Length > 0);
            case "guandu:suishi":
                if (c.TargetSeat is not { } other || other == owner.Seat)
                    return false;
                return c.Window == SkillProgramTriggerWindow.DyingEntering ? _coreDyingCursorRuntimeOperations.Value.ActiveDying is { DamageFrameId: not null } dying && dying.VictimSeat == other && _coreNativeDamageHandlesOperations.Value.GetDyingAttack(dying)is { IsSourceLess: false } causal && _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[causal.SourceSeat])is { } faction && faction == _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(owner) : _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[other])is { } deadFaction && deadFaction == _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(owner);
            case "guandu:moushi":
                return _coreGuanduGeneralNativeCardsOperations.GuanduMoushiQualifies(gate.OwnerSeat, gate.Candidate.SkillInstanceId, c);
            default:
                return true;
        }
    }

    internal void IssueGuanduAllCardBan(ProgramSkillFrame f, int target)
    {
        if (!_coreGuanduActivitiesOperations.IsGuanduActivity || f.GuanduGeneral?.Operation != SkillProgramEffectOp.ResolveGuanduXiying || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<GuanduAllCardBanIssuedEvent>().Any(e => e.ProgramFrameId == f.Id && e.TargetSeat == target))
            throw new InvalidOperationException("Xiying requires its exact once-paid other-character decision.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduAllCardBanIssuedEvent(f.Id, f.OwnerSeat, target, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f)));
    }

    internal bool IsGuanduCardActionForbidden(int actorSeat, CardKind kind, CardActionType type) => _coreGuanduActivitiesOperations.IsGuanduActivity && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<GuanduAllCardBanIssuedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TargetSeat == actorSeat);
    internal bool TryGetGuanduIncomingHpReplacement(GameEngine.IDamageAttempt attack, out CardUseEffectSource source)
    {
        source = null !;
        if (!_coreGuanduActivitiesOperations.IsGuanduActivity || attack.IsSourceLess || attack.SourceSeat == attack.TargetSeat || attack.DamageAmount <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.TargetSeat) || !_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].IsAlive || !_coreGuanduGeneralsOperations.GuanduSkillEnabled(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat], "guandu:gangzhi"))
            return false;
        var grant = _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat]).ActiveGrants.First(g => g.SkillId == "guandu:gangzhi");
        source = new("guandu:gangzhi", "incoming-damage", attack.TargetSeat, grant.SkillInstanceId);
        return true;
    }

    [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 20)]
    internal bool GuanduCardUseForbidden(ProgramCardUseQuery query) => IsGuanduCardActionForbidden(query.ActorSeat, query.Kind, query.ActionType);
}
