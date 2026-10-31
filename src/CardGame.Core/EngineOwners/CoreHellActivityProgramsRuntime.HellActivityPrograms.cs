#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreHellActivityProgramsRuntime
{
    SkillProgramStepOutcome IHellActivityProgramHost.ExecuteHellActivity(ProgramSkillFrame f, SkillProgramEffectOp op) => ExecuteHellActivity(f, op);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 40110)]
    internal bool AllowsHellActivityTier(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId is not ("hell:player-tier" or "hell:boss-tier"))
            return true;
        return _coreHellEncounterHostOperations.HellFeature?.AllowsTierTrigger(gate.SkillId, gate.Candidate.BindingId, gate.Owner.TeamId) ?? false;
    }

    internal SkillProgramStepOutcome ExecuteHellActivity(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        switch (operation)
        {
            case SkillProgramEffectOp.GainHellTurnSlash:
                if (_coreHellEncounterHostOperations.HellFeature?.AllowsTierTrigger("hell:boss-tier", "turn-slash", owner.TeamId) != true || frame.WindowContext?.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow || _coreActualTurnProgressTurnProgressionAccess._turnProgression.OwnerSeat != owner.Seat)
                    throw new InvalidOperationException("Hell tier Slash lost its actual turn owner.");
                var slash = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).FirstOrDefault(card => card.Kind == CardKind.Slash) ?? _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).FirstOrDefault(card => card.Kind == CardKind.Slash);
                if (slash is not null)
                    _corePhysicalMovementFacadesOperations.MoveCard(slash, _cardZonesOperations._cardZones.GetLocation(slash.Id), CardLocation.Hand(owner.Seat), new("hell.tier.obtain-slash"));
                return SkillProgramStepOutcome.Continue;
            case SkillProgramEffectOp.DrawHellChongshengRefill:
                var count = Math.Max(0, Math.Min(owner.MaxHp, 5) - _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HellChongshengRefillIssuedEvent(frame.Id, owner.Seat, count));
                return _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, owner.Seat, count, new("hell.chongsheng.refill"));
            case SkillProgramEffectOp.RecoverHellChongsheng:
                _coreProgramLifecycleOperations.RecoverProgramTargetTo(frame.Id, owner.Seat, owner.Seat, SkillProgramNumberExpression.IntegerConstant, owner.MaxHp, true);
                return SkillProgramStepOutcome.Continue;
            default:
                throw new InvalidOperationException("Unknown Hell activity operation.");
        }
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.DrawHellChongshengRefill)]
    internal void ReturnHellChongshengDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome)
    {
        var issued = _coreProgramEventHistoryOperations.ProgramEventHistory<HellChongshengRefillIssuedEvent>().Single(e => e.ProgramFrameId == frame.Id);
        if (issued.OwnerSeat != frame.OwnerSeat || outcome.RecipientSeat != frame.OwnerSeat || outcome.RequestedCount != issued.RequestedCount)
            throw new InvalidOperationException("Chongsheng refill changed its exact paid draw request.");
        _coreProgramLifecycleOperations.CompleteProgramDrawCards(frame.Id, frame.OwnerSeat, null, SkillProgramCardSetVisibility.Private, new("hell.chongsheng.refill"), 0, outcome.ActualDrawIds);
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }
}
