#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceLiuYanTrioRuntime
{
    [ProgramDistanceRule(ProgramDistanceStage.DistanceBaseTerms, 24630)]
    internal void AddSourceJingqiDistanceTerms(ProgramDistanceQuery query, List<RuleQueryBaseTerm> terms)
    {
        if (!query.Source.IsAlive || !query.Target.IsAlive || query.Source.Seat == query.Target.Seat)
            return;
        foreach (var holder in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _coreTeamActivitiesOperations.SamePublicTeam(p, query.Source) && _coreTeamActivityProgramsOperations.PublicTeamEnemy(p.Seat, query.Target) && _coreNationalSkillsOperations.HasRuntimeSkill(p, "shanhe:source-jingqi-1161")))
            terms.Add(new($"source-jingqi:{(holder.Seat)}:{(_coreNationalSkillsOperations.GetRuntimeSkillInstanceId(holder, "shanhe:source-jingqi-1161"))}", -1));
    }

    SkillProgramStepOutcome IShanheSourceXiechanHost.ResolvePindianDuel(ProgramSkillFrame frame) => ResolveSourceXiechanDuel(frame);
    internal SkillProgramStepOutcome ResolveSourceXiechanDuel(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != ShanheSourceXiechanComposition.Skill || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 2 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var opponent] || f.PindianResultBindings.SingleOrDefault(b => b.Name == "contest")is not { NativePindianFrameId: { } pindianId } bound || bound.SourceSeat != f.OwnerSeat || bound.OpponentSeat != opponent || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat)
            throw new InvalidOperationException("Source4055 requires its actual limited Play activation and returned original Pindian.");
        var determined = _coreProgramEventHistoryOperations.ProgramEventHistory<PindianResultDeterminedEvent>().SingleOrDefault(e => e.FrameId == pindianId && e.SkillId == f.SkillId) ?? throw new InvalidOperationException("Source4055 lost its original actual Pindian result event.");
        var result = determined.Result;
        if (result.SourceSeat != bound.SourceSeat || result.OpponentSeat != bound.OpponentSeat || result.SourceRank != bound.SourceRank || result.OpponentRank != bound.OpponentRank || result.SourceWon != bound.SourceWon)
            throw new InvalidOperationException("Source4055 cannot replace its frozen Pindian result or directional participants.");
        var actor = result.SourceWon ? f.OwnerSeat : opponent;
        var target = result.SourceWon ? opponent : f.OwnerSeat;
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreDamageColorDuelProgramsOperations.CanIssueSelectedActorDuel(target, actor))
            return SkillProgramStepOutcome.Continue;
        var receipt = new ShanheSourceXiechanDuelReceipt(pindianId, result.SourceCardId, result.OpponentCardId, result.SourceRank, result.OpponentRank, result.SourceWon, actor, target, _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1);
        var origin = new ProgramSelectedActorDuelOrigin(f.Id, f.InstructionIndex, f.OwnerSeat, actor, f.SkillId, f.ActivationId, f.SkillInstanceId, f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)
        {
            InitialTargetSeat = target,
            ShanheSourceXiechan = receipt
        };
        var card = new Card(0, CardKind.Duel, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor, [target], CardKind.Duel, physicalCardIds: [], selectedActorDuelOrigin: origin);
        if (id != receipt.UseId)
            throw new InvalidOperationException("Source4055's typed Duel did not retain its allocated native use identity.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceXiechanDuelIssuedEvent(f.Id, id, pindianId, f.OwnerSeat, actor, target, result.SourceCardId, result.OpponentCardId, result.SourceWon));
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor, [target], LegalActionKind.Duel, playedCardKind: CardKind.Duel);
        return SkillProgramStepOutcome.AwaitChild;
    }
}
