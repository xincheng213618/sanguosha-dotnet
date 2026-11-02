#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramTargetRulesRuntime
{
    bool IProgramTargetQuotaQueries.HasTargetCardQuotaAllowance(int actorSeat, int targetSeat) =>
        TargetRules.HasQuotaAllowance(new(actorSeat, targetSeat));
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaAfterLimit, 110)]
    internal bool GeneralQuotaAllowsSlash(ProgramSlashQuotaQuery query) =>
        TargetRules.HasQuotaAllowance(new(query.Actor.Seat, query.Target.Seat));
    internal ProgramTargetRules TargetRules => _targetRulesOperations._targetRules ??= new(CoreProgramTargetRulesDefinitions.TargetRuleMetadata.Value, _kernelCoreProgramRuntimeModulesOperations.BindRuntimeHandler);
}
