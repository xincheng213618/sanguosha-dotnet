#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheTerrainPoliciesEntries
{
    internal RuleQueryEvaluation ApplyMinimumHandLimitPolicy(CharacterState player, RuleQueryEvaluation ordinary) => _resolve().ApplyMinimumHandLimitPolicy(player, ordinary);
    internal bool IsMaximumOwnPlayCardUseForbidden(ProgramCardUseQuery query) => _resolve().IsMaximumOwnPlayCardUseForbidden(query);
}
