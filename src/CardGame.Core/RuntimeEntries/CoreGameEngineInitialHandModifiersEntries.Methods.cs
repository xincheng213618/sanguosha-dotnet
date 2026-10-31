#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineInitialHandModifiersEntries
{
    internal global::CardGame.Core.RuleQueryEvaluation EvaluateInitialHandSize(global::CardGame.Core.CharacterState player) => _resolve().EvaluateInitialHandSize(player);
    internal int GetInitialHandSize(global::CardGame.Core.CharacterState player) => _resolve().GetInitialHandSize(player);
}
