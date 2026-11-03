#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterAHuiNanOlRuntime
{
    internal bool ValidAHuiNan(ProgramSkillFrame f)
    {
        if (f.AHuiNanJueman is not { } r || !HasAHuiNanCapability || !AHuiNanStartedBinding(f) || r.Source != AHuiNanSource(f) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != 1 || f.InstructionIndex != 1 || r.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.PendingMovementContinuation is not null || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || !r.History.SequenceEqual(AHuiNanHistory(r.ActualTurn)))
            return false;
        var own = r.History.Take(2).Count(e => e.ActorSeat == f.OwnerSeat);
        if (r.History.Count > 3 || r.History.Any(e => !AHuiNanBasicKind(e.Kind)) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AHuiNanJuemanStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var start] || start != new AHuiNanJuemanStartedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwner, r.History.Count, own, r.History.Count == 3 ? r.History[2].Kind : null))
            return false;
        if (r.History.Count != 3 || own != 0)
            return false;
        var issued = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AHuiNanJuemanUseIssuedEvent>().Where(e => e.Origin.ProgramFrameId == f.Id).ToArray();
        var returned = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<AHuiNanJuemanUseReturnedEvent>().Where(e => e.Origin.ProgramFrameId == f.Id).ToArray();
        if (issued.Length > 1 || returned.Length > 1 || returned.Any(e => issued.Count(i => i.Origin == e.Origin) != 1))
            return false;
        if (r.Stage is AHuiNanJuemanStage.NativeUse or AHuiNanJuemanStage.NativeChildren)
            return r.Use is { } o && issued is [var issue] && issue.Origin == o && o.ProgramFrameId == f.Id && o.Source == r.Source && o.GameplayHash == r.GameplayHash && o.ActualTurn == r.ActualTurn && o.ActorSeat == f.OwnerSeat && o.CardKind == r.History[2].Kind && (CoreNativeCardClassificationsRuntime.IsSlashCard(o.CardKind) || o.CardKind is CardKind.Peach or CardKind.Alcohol) && returned.Length == (r.Stage == AHuiNanJuemanStage.NativeChildren ? 1 : 0);
        return r.Use is null && issued.Length == 0 && returned.Length == 0;
    }
}
