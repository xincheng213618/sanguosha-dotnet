#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramMultiTargetDuelsEntries
{
    internal void AddProgramMultiTargetDuelActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor) => _resolve().AddProgramMultiTargetDuelActions(actions, actor);
    internal global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.IReadOnlyList<int>> EnumerateProgramDuelTargets(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<int> candidates, int minimum = 1) => _resolve().EnumerateProgramDuelTargets(actor, candidates, minimum);
    internal bool HasProgramIronChainTargetExpansion(int sourceSeat) => _resolve().HasProgramIronChainTargetExpansion(sourceSeat);
    internal void ResolveProgramMultiTargetDuel(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card duel, global::CardGame.Core.LegalAction action) => _resolve().ResolveProgramMultiTargetDuel(source, duel, action);
}
