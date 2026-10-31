#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHpOrderedDistancePoliciesEntries
{
    internal RuleQueryEvaluation ApplyHpOrderedDistance(CharacterState source, CharacterState target, RuleQueryEvaluation original) => _resolve().ApplyHpOrderedDistance(source, target, original);
}
