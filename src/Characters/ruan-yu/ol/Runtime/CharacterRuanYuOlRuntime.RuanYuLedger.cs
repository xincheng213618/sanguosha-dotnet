#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterRuanYuOlRuntime
{
    internal IEnumerable<XingzuoEndingIssuedEvent> CurrentXingzuoDebts() => !HasRuanYuCapability ? [] : _coreProgramEventHistoryOperations.ProgramEventHistory<XingzuoEndingIssuedEvent>().Where(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && e.Source.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && _coreMatchParticipantsPlayersAccess._players[e.Source.OwnerSeat].IsAlive && _contentRegistryOperations._contentRegistry.GetSkill(e.Source.SkillId).Program?.GameplayHash == e.GameplayHash && !_coreProgramEventHistoryOperations.ProgramEventHistory<XingzuoEndingConsumedEvent>().Any(c => c.DebtId == e.DebtId));
    internal void AppendRuanYuXingzuoEndingItems(List<TurnEndingBoundaryItem> items)
    {
        foreach (var debt in CurrentXingzuoDebts().OrderBy(e => e.DebtId))
        {
            var t = _contentRegistryOperations._contentRegistry.GetSkill("ol:xingzuo").Program!.Triggers.Single(t => t.Id == "issued-ending");
            var c = new ProgramTriggerCandidate(debt.Source.OwnerSeat, "ol:xingzuo", t.Id, debt.Source.SkillInstanceId, debt.GameplayHash, t.Priority, items.Count);
            items.Add(new(TurnEndingBoundaryItemKind.Program, t.Priority, "ruan-yu.xingzuo:" + debt.DebtId, c, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat])));
        }
    }

    internal XingzuoEndingIssuedEvent? CurrentRuanYuEndingDebt(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!HasRuanYuCapability || c.SkillId != "ol:xingzuo" || c.BindingId != "issued-ending" || context.Window != SkillProgramTriggerWindow.TurnEnding || context.OwnerSeat != c.OwnerSeat || c.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || context.SourceSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || context.TargetSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || context.OccurrenceIndex != c.OccurrenceIndex || _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(f => f.Id == context.ParentFrameId)is not { } parent || parent.OwnerSeat != c.OwnerSeat || parent.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber || parent.ItemIndex < 0 || parent.ItemIndex >= parent.Items.Count || parent.Items[parent.ItemIndex].Candidate != c)
            return null;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<XingzuoEndingIssuedEvent>().SingleOrDefault(e => parent.Items[parent.ItemIndex].StableIdentity == "ruan-yu.xingzuo:" + e.DebtId && e.Source.SkillId == c.SkillId && e.Source.BindingId == "private-bottom" && e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillInstanceId == c.SkillInstanceId && e.GameplayHash == c.GameplayHash && e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && _coreProgramEventHistoryOperations.ProgramEventHistory<RuanYuStartedEvent>().Count(s => s.FrameId == e.DebtId && s.Source == e.Source && s.GameplayHash == e.GameplayHash && s.Op == SkillProgramEffectOp.ResolveXingzuoPrivateBottomExchange && s.ActualTurn == e.ActualTurn) == 1);
    }

    [ProgramIssuedTrigger(470)]
    internal bool IsIssuedRuanYuXingzuoEndingCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreTableSeatQueriesOperations.IsValidPlayerSeat(c.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && CurrentRuanYuEndingDebt(c, context)is { } debt && (!_coreProgramEventHistoryOperations.ProgramEventHistory<XingzuoEndingConsumedEvent>().Any(e => e.DebtId == debt.DebtId) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.RuanYu?.DebtId == debt.DebtId && f.WindowContext == context && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c)));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27430)]
    internal bool CanOfferRuanYu(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (t.Effects is not [{ } e] || !RuanYuComposition.Handles(e.Op))
            return true;
        return e.Op switch
        {
            SkillProgramEffectOp.ResolveXingzuoPrivateBottomExchange => context.Window == SkillProgramTriggerWindow.PlayPhaseStarting && c.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat], c.SkillId, c.SkillInstanceId),
            SkillProgramEffectOp.ResolveXingzuoIssuedEndingExchange => IsIssuedRuanYuXingzuoEndingCandidate(c, context),
            SkillProgramEffectOp.ResolveMiaoxianUniqueRedUseDraw => IsIssuedRuanYuMiaoxianRedCandidate(c, context),
            _ => false
        };
    }
}
