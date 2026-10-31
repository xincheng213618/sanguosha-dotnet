#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePlannedPhaseQuantitiesRuntime
{
    internal void AdjustProgramNormalDrawByPlannedCount(ProgramSkillFrame frame)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame active || active.Id != frame.Id || frame.TriggerId is null || _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program is not { } program || program.GameplayHash != frame.GameplayHash || frame.InstructionIndex < 1 || ProgramInstructionResolver.Default.Resolve(frame, program).Instructions.ElementAtOrDefault(frame.InstructionIndex - 1)is not { Op: SkillProgramEffectOp.AdjustNormalDraw, NumberExpression: SkillProgramNumberExpression.PlannedNormalDrawCount })
            throw new InvalidOperationException("A planned normal-draw adjustment requires its exact active instruction.");
        // A replacement owns all normal draws. A zero remaining budget also
        // stays zero; neither case issues a second payment or revives the plan.
        var count = _coreProgramLifecycleOperations.GetProgramPlannedNormalDrawCount(frame, allowReplacedNoOp: true);
        if (count > 0)
            _coreProgramLifecycleOperations.AdjustProgramNormalDraw(frame, count);
    }

    internal int GetDiscardQuotaMultiplier(CharacterState owner) => CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(RuleQueryService.Evaluate(SkillRuleQuery.DiscardQuotaMultiplier, new RuleQueryBounds(1, 20), [new RuleQueryBaseTerm("phase:discard:default-quota-multiplier", 1)], _coreRuleQueriesOperations.CollectNumericRuleContributions(owner, SkillRuleQuery.DiscardQuotaMultiplier)));
}
