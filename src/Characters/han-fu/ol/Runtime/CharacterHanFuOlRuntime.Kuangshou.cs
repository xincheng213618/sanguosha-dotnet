#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHanFuOlRuntime
{
    internal int[] KuangshouDamagedSeats(int cutoff)
    {
        var events = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Take(cutoff).ToArray();
        var start = Array.FindLastIndex(events, e => e is TurnStartedEvent t && t.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber);
        return events.Skip(start + 1).OfType<DamageAppliedEvent>().Where(e => e.Amount > 0).Select(e => e.TargetSeat).Distinct().Order().ToArray();
    }

    internal void FreezeKuangshouDiscard(ProgramSkillFrame f)
    {
        var cutoff = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Count();
        var seats = KuangshouDamagedSeats(cutoff);
        var candidates = HanFuDiscardCards(f);
        var required = Math.Min(seats.Length, candidates.Count);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HanFu = f.HanFu!with { Stage = HanFuStage.DiscardChoice, HistoryCutoff = cutoff, DamagedSeats = seats, RequiredDiscard = required, DiscardCandidates = candidates, SelectedDiscard = [] } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KuangshouQuotaFrozenEvent(f.Id, cutoff, seats.Length, required));
        if (required == 0)
        {
            PayHanFuCards(f, [], CardLocation.DiscardPile, KuangshouDiscardReason, HanFuStage.DiscardChildren);
            ContinueHanFu(f.Id);
        }
        else
            PublishHanFuChoice(f);
    }
}
