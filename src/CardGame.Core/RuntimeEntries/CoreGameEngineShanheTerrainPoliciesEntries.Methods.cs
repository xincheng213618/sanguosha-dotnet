#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheTerrainPoliciesEntries
{
    internal global::CardGame.Core.RuleQueryEvaluation ApplyMinimumHandLimitPolicy(global::CardGame.Core.CharacterState player, global::CardGame.Core.RuleQueryEvaluation ordinary) => _resolve().ApplyMinimumHandLimitPolicy(player, ordinary);
    internal bool IsMaximumOwnPlayCardUseForbidden(global::CardGame.Core.ProgramCardUseQuery query) => _resolve().IsMaximumOwnPlayCardUseForbidden(query);
}
