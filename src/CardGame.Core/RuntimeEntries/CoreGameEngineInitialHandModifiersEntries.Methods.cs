#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineInitialHandModifiersEntries
{
    internal RuleQueryEvaluation EvaluateInitialHandSize(CharacterState player) => _resolve().EvaluateInitialHandSize(player);
    internal int GetInitialHandSize(CharacterState player) => _resolve().GetInitialHandSize(player);
}
