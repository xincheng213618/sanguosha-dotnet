#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWenYangOlRuntime
{
    internal bool WenYangDebtConsumed(long id) => _coreProgramEventHistoryOperations.ProgramEventHistory<WenYangQingjiaoDebtConsumedEvent>().Any(e => e.DebtId == id);
    internal bool ValidWenYangDebt(WenYangQingjiaoDebtIssuedEvent debt) => debt.Source.SkillId == "ol:qingjiao" && _coreTableSeatQueriesOperations.IsValidPlayerSeat(debt.Source.OwnerSeat) && debt.TurnOwner == debt.Source.OwnerSeat && debt.ActualTurn > 0 && debt.Gained is >= 0 and <= 8 && _contentRegistryOperations._contentRegistry.GetSkill(debt.Source.SkillId).Program?.GameplayHash == debt.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<WenYangQingjiaoDebtIssuedEvent>().Count(e => e.FrameId == debt.FrameId) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WenYangStartedEvent>().Count(e => e.FrameId == debt.FrameId && e.Source == debt.Source && e.GameplayHash == debt.GameplayHash && e.ActualTurn == debt.ActualTurn && e.TurnOwner == debt.TurnOwner && e.Op == SkillProgramEffectOp.ResolveQingjiaoWholeHandExchange) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WenYangMovementIssuedEvent>().Count(e => e.FrameId == debt.FrameId && e.Step == 1 && e.Stage == WenYangStage.HandDiscardChildren) == 1;
    internal void AppendWenYangEndingItems(List<TurnEndingBoundaryItem> items)
    {
        if (!HasWenYangCapability)
            return;
        foreach (var debt in _coreProgramEventHistoryOperations.ProgramEventHistory<WenYangQingjiaoDebtIssuedEvent>().Where(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && !WenYangDebtConsumed(e.FrameId)).ToArray())
        {
            if (!ValidWenYangDebt(debt) || !_coreMatchParticipantsPlayersAccess._players[debt.Source.OwnerSeat].IsAlive)
                continue;
            var t = _contentRegistryOperations._contentRegistry.GetSkill(debt.Source.SkillId).Program!.Triggers.Single(t => t.Effects is [{ Op: SkillProgramEffectOp.ResolveQingjiaoIssuedEndingDiscard }]);
            var c = new ProgramTriggerCandidate(debt.Source.OwnerSeat, debt.Source.SkillId, t.Id, debt.Source.SkillInstanceId, debt.GameplayHash, t.Priority, items.Count);
            items.Add(new(TurnEndingBoundaryItemKind.Program, t.Priority, $"wen-yang-ending:{(debt.FrameId)}", c, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat])));
        }
    }

    internal WenYangQingjiaoDebtIssuedEvent? CurrentWenYangDebt(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!HasWenYangCapability || c.SkillId != "ol:qingjiao" || c.BindingId != "issued-actual-ending" || context.Window != SkillProgramTriggerWindow.TurnEnding || context.OwnerSeat != c.OwnerSeat || context.SourceSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || context.TargetSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || c.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || context.OccurrenceIndex != c.OccurrenceIndex || _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } parent || parent.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || parent.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || parent.ItemIndex < 0 || parent.ItemIndex >= parent.Items.Count || parent.Items[parent.ItemIndex].Candidate != c)
            return null;
        var debt = _coreProgramEventHistoryOperations.ProgramEventHistory<WenYangQingjiaoDebtIssuedEvent>().SingleOrDefault(e => parent.Items[parent.ItemIndex].StableIdentity == $"wen-yang-ending:{(e.FrameId)}");
        return debt is not null && ValidWenYangDebt(debt) && debt.Source.OwnerSeat == c.OwnerSeat && debt.Source.SkillId == c.SkillId && debt.Source.SkillInstanceId == c.SkillInstanceId && debt.GameplayHash == c.GameplayHash && debt.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && debt.TurnOwner == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat ? debt : null;
    }

    [ProgramIssuedTrigger(370)]
    internal bool IsIssuedWenYangQingjiaoEndingCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreTableSeatQueriesOperations.IsValidPlayerSeat(c.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && CurrentWenYangDebt(c, context)is { } debt && (!WenYangDebtConsumed(debt.FrameId) || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.WenYang is { Op: SkillProgramEffectOp.ResolveQingjiaoIssuedEndingDiscard } r && r.DebtId == debt.FrameId && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c) && f.WindowContext == context));
}
