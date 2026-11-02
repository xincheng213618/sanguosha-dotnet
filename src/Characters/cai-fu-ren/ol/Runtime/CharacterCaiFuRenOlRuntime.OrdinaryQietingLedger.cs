#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaiFuRenOlRuntime
{
    internal bool IsOrdinaryCaiFuRenQietingTrigger(SkillProgramTrigger trigger) => trigger.Effects is [{ Op: SkillProgramEffectOp.ResolveOrdinaryCaiFuRenQieting or SkillProgramEffectOp.ResolveBoundaryMobileQieting }];
    internal bool IsEligibleOrdinaryCaiFuRenQietingCandidate(DeferredTurnEndFrame parent, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        return CharacterCaiFuRenBoundaryMobileRuntime.IsKnownCaiFuRenQietingBinding(candidate.SkillId, candidate.BindingId) && IsOrdinaryCaiFuRenQietingTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) && context.Window == SkillProgramTriggerWindow.AfterTurnEnded && _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(parent) && parent.Id == context.ParentFrameId && parent.AfterTurnEnded is { } w && w.ItemIndex >= 0 && w.ItemIndex < w.Items.Count && w.Items[w.ItemIndex].Candidate == candidate && context.Facts == w.Items[w.ItemIndex].Facts && context.OwnerSeat == candidate.OwnerSeat && context.SourceSeat == parent.OwnerSeat && context.TargetSeat == parent.OwnerSeat && context.OccurrenceIndex == candidate.OccurrenceIndex && _coreTableSeatQueriesOperations.IsValidPlayerSeat(candidate.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && candidate.OwnerSeat != parent.OwnerSeat && context.HistoryPoint is { } c && c.ActualTurn == parent.TurnNumber && c.TurnOwnerSeat == parent.OwnerSeat && c.Cursor >= 0 && c.Cursor <= history.Length && !_characterCaiFuRenBoundaryMobileOperations.CaiFuRenQietingDisqualified(candidate.SkillId, c) && history.Take((int)c.Cursor).OfType<TurnStartedEvent>().Any(e => e.TurnNumber == c.ActualTurn && e.ActorSeat == c.TurnOwnerSeat) && history.Take((int)c.Cursor).OfType<TurnEndedEvent>().Any(e => e.TurnNumber == c.ActualTurn && e.ActorSeat == c.TurnOwnerSeat);
    }
}
