#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActualTurnViewAsRuntime
{
    internal static string ActualTurnViewAsUsageId(SkillProgramViewAs rule) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_ActualTurnViewAs.ActualTurnViewAsUsageId(rule);
    internal bool CanUseActualTurnLimitedViewAs(IndexedSkillProgramInstance instance, SkillProgramViewAs rule, CharacterState owner) => rule.UsesPerActualTurn is not { } limit || _skillRuntimeStateOperations._skillRuntimeState.GetUsage(owner.Seat, instance.SkillId, ActualTurnViewAsUsageId(rule), SkillUsageScope.Turn) < limit;
    internal void ConsumeActualTurnLimitedViewAs(CardConversionSource source)
    {
        if (_corePlayCardHistoryOperations.ViewAsRule(source)is not { UsesPerActualTurn: { } limit } rule)
            return;
        var usageId = ActualTurnViewAsUsageId(rule);
        if (!_skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(source.OwnerSeat, source.SkillId, usageId, SkillUsageScope.Turn, limit))
            throw new InvalidOperationException("The actual-turn conversion allowance was already consumed.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SkillUsageConsumedEvent(source.OwnerSeat, source.SkillId, usageId, SkillUsageScope.Turn, 1));
    }

    [ProgramCardLegality(ProgramCardLegalityStage.ConversionUsageBan, 20)]
    internal bool ActualTurnConversionUsageUnavailable(ProgramConfiguredViewAsQuery query) => !CanUseActualTurnLimitedViewAs(query.Instance, query.Rule, query.Owner);
}
