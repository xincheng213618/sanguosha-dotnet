#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaiFuRenOlRuntime
{
    internal bool IsOrdinaryCaiFuRenQietingTrigger(SkillProgramTrigger trigger) => trigger.Effects is [{ Op: SkillProgramEffectOp.ResolveOrdinaryCaiFuRenQieting or SkillProgramEffectOp.ResolveBoundaryMobileQieting }];
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 27596)]
    internal void ObserveOrdinaryQietingUses(IGameEvent e)
    {
        if (!HasOrdinaryCaiFuRenCapability || e is OrdinaryQietingActualUseEvent || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber <= 0)
            return;
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().ToArray())
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Any(d => d.ResolutionId == use.Id))
                RecordOrdinaryQietingUse(use);
    }

    [NativeUsePreparation(27596)]
    internal bool CaptureOrdinaryQietingFinalUse(CardUseFrame use)
    {
        if (HasOrdinaryCaiFuRenCapability && _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Any(d => d.ResolutionId == use.Id))
            RecordOrdinaryQietingUse(use);
        return false;
    }

    internal void RecordOrdinaryQietingUse(CardUseFrame use)
    {
        if (use.Action is { Type: not CardActionType.Use })
            return;
        var targets = use.Action?.EffectiveDesignatedTargetSeats ?? use.TargetSeats;
        var old = _coreProgramEventHistoryOperations.ProgramEventHistory<OrdinaryQietingActualUseEvent>().LastOrDefault(e => e.NativeUseId == use.Id);
        var next = new OrdinaryQietingActualUseEvent(old?.ActualTurn ?? _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, old?.TurnOwnerSeat ?? _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, use.Id, use.Action?.ActionId, use.SourceSeat, targets.Any(t => t != use.SourceSeat), targets.Distinct().Count());
        if (next != old)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(next);
    }

    internal bool OrdinaryQietingUsedOnAnother(OrdinaryQietingTurnContext c) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Take(checked((int)c.HistoryCutoff)).OfType<OrdinaryQietingActualUseEvent>().Where(e => e.ActualTurn == c.ActualTurn && e.TurnOwnerSeat == c.TurnOwnerSeat).GroupBy(e => e.NativeUseId).Select(g => g.Last()).Any(e => e.ActorSeat == c.TurnOwnerSeat && e.UsedOnAnother);
    internal ProgramSkillWindowContext CreateOrdinaryCaiFuRenQietingContext(DeferredTurnEndFrame parent, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (!global::CardGame.Core.EngineOwners.CharacterCaiFuRenBoundaryMobileRuntime.IsKnownCaiFuRenQietingBinding(candidate.SkillId, candidate.BindingId) || !IsOrdinaryCaiFuRenQietingTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)))
            return context;
        var c = new OrdinaryQietingTurnContext(parent.TurnNumber, parent.OwnerSeat, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Count(), false);
        return context with
        {
            OrdinaryQieting = c with
            {
                UsedOnAnother = OrdinaryQietingUsedOnAnother(c)
            }
        };
    }

    internal bool IsEligibleOrdinaryCaiFuRenQietingCandidate(DeferredTurnEndFrame parent, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        return global::CardGame.Core.EngineOwners.CharacterCaiFuRenBoundaryMobileRuntime.IsKnownCaiFuRenQietingBinding(candidate.SkillId, candidate.BindingId) && IsOrdinaryCaiFuRenQietingTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)) && context.Window == SkillProgramTriggerWindow.AfterTurnEnded && _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(parent) && parent.Id == context.ParentFrameId && parent.AfterTurnEnded is { } w && w.ItemIndex >= 0 && w.ItemIndex < w.Items.Count && w.Items[w.ItemIndex].Candidate == candidate && context.Facts == w.Items[w.ItemIndex].Facts && context.OwnerSeat == candidate.OwnerSeat && context.SourceSeat == parent.OwnerSeat && context.TargetSeat == parent.OwnerSeat && context.OccurrenceIndex == candidate.OccurrenceIndex && _coreTableSeatQueriesOperations.IsValidPlayerSeat(candidate.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && candidate.OwnerSeat != parent.OwnerSeat && context.OrdinaryQieting is { } c && c.ActualTurn == parent.TurnNumber && c.TurnOwnerSeat == parent.OwnerSeat && c.HistoryCutoff >= 0 && c.HistoryCutoff <= history.Length && !_characterCaiFuRenBoundaryMobileOperations.CaiFuRenQietingDisqualified(candidate.SkillId, c) && history.Take((int)c.HistoryCutoff).OfType<TurnStartedEvent>().Any(e => e.TurnNumber == c.ActualTurn && e.ActorSeat == c.TurnOwnerSeat) && history.Take((int)c.HistoryCutoff).OfType<TurnEndedEvent>().Any(e => e.TurnNumber == c.ActualTurn && e.ActorSeat == c.TurnOwnerSeat) && !history.Skip((int)c.HistoryCutoff).OfType<OrdinaryQietingActualUseEvent>().Any(e => e.ActualTurn == c.ActualTurn && e.TurnOwnerSeat == c.TurnOwnerSeat);
    }
}
