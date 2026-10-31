#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTurnDiscardSuitHistoryRuntime
{
    internal bool TracksTurnDiscardSuits => _contentRegistryOperations._contentRegistry.ProgramDependencies.UsesTriggerCondition(SkillProgramTriggerConditionKind.TurnDiscardIncludesAllSuits) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveMobileShiXiongtu);
    internal int CurrentTurnDiscardSuitMask => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnDiscardSuitMaskChangedEvent>().LastOrDefault(e => e.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber)?.SuitMask ?? 0;

    internal void CaptureTurnDiscardSuitFact(int turn, IReadOnlyList<CardMovementRecord> movements)
    {
        if (!TracksTurnDiscardSuits || turn <= 0)
            return;
        if (turn != _coreActualTurnProgressTurnNumberAccess._turnNumber)
            throw new InvalidOperationException("A discard batch lost its actual turn.");
        var previousMask = CurrentTurnDiscardSuitMask;
        var mask = previousMask;
        foreach (var move in movements.Where(m => m.To == CardLocation.DiscardPile && m.From != CardLocation.DiscardPile))
        {
            var suit = move.PrintedSuitAtMovement(_cardZonesOperations._cardZones);
            mask |= suit switch
            {
                Suit.Spade => 1,
                Suit.Heart => 2,
                Suit.Club => 4,
                Suit.Diamond => 8,
                _ => 0
            };
        }

        if (mask != previousMask)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TurnDiscardSuitMaskChangedEvent(turn, mask));
    }

    internal int? ProgramTriggerUsageLimit(SkillProgramTrigger trigger) => _programTriggerOperations.Get().ProgramTriggerUsageLimit(_kernelCoreProgramTriggerRuntimeQueriesOperations.CreateProgramTriggerQueries(), trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CaptureMovementBatchFacts, 300)]
    internal void CaptureMovementBatchFactsCaptureTurnDiscardSuitFact(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements) => CaptureTurnDiscardSuitFact(batch.TurnNumber, movements);
}
