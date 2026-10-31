#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterNiuJinOlRuntime
{
    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeJudgmentPhase, 10)]
    internal bool ConsumeNiuJinJudgmentSkip()
    {
        // This is an already issued next-phase obligation. A later source loss
        // cannot defer it until that skill is acquired again. A face-down whole
        // turn skips this boundary, so the debt waits for the next real turn.
        var debts = _coreGameEngineProgramPersistentStateProgramBooleanStatesAccess._programBooleanStates.Where(pair => pair.Value && pair.Key.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && pair.Key.SkillId == "ol:cuorui" && pair.Key.StateId == "skip-next-judgment").Select(pair => pair.Key).ToArray();
        foreach (var debt in debts)
        {
            var alreadySkipped = _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects.HasFlag(GameEngine.DelayedTurnEffects.SkipJudgmentPhase);
            _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects |= GameEngine.DelayedTurnEffects.SkipJudgmentPhase;
            _coreProgramPersistentStateOperations.SetProgramBooleanState(debt.OwnerSeat, debt.SkillId, debt.SkillInstanceId, debt.StateId, false);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NiuJinJudgmentSkipConsumedEvent(debt.OwnerSeat, debt.SkillInstanceId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, alreadySkipped));
        }

        // The native turn transition still owns progression and publication.
        return false;
    }
}
