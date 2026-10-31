#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXieLingYuOlRuntime
{
    internal bool HasXieLingYuCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveYuandiDiscardNames) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveXinyouBasicCycle);

    internal static CardConversionSource XieLingYuSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_xie_ling_yu_ol_XieLingYuLedger.XieLingYuSource(f);
    internal static int XieLingYuNameBit(CardKind kind) => RuntimeSkills.RuntimeRules_xie_ling_yu_ol_XieLingYuLedger.XieLingYuNameBit(kind);
    internal static CardKind[] XieLingYuNames(int mask) => RuntimeSkills.RuntimeRules_xie_ling_yu_ol_XieLingYuLedger.XieLingYuNames(mask);
    internal void CaptureXieLingYuMovementBatch(CardMovementBatchContext batch)
    {
        if (!HasXieLingYuCapability || batch.TurnNumber <= 0)
            return;
        foreach (var m in batch.Movements.Where(m => m.To == CardLocation.DiscardPile && m.From != m.To && XieLingYuNameBit(m.CardKind)is> 0 and < 8))
        {
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<YuandiDiscardEnteredEvent>().Any(e => e.MovementSequence == m.Sequence))
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YuandiDiscardEnteredEvent(batch.Id, batch.TurnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, m.Sequence, m.CardId, m.CardKind, CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(m.CardKind), _coreNativeUsedDiscardOriginsOperations.IsNativeTrueUseDiscardMovement(batch, m)));
        }
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 5000)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureXieLingYuMovementBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureXieLingYuMovementBatch(completed);
        return completed;
    }
}
