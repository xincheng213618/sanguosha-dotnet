#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramMultiTargetDuelsEntries
{
    internal void AddProgramMultiTargetDuelActions(ICollection<LegalAction> actions, CharacterState actor) => _resolve().AddProgramMultiTargetDuelActions(actions, actor);
    internal IEnumerable<IReadOnlyList<int>> EnumerateProgramDuelTargets(CharacterState actor, IReadOnlyList<int> candidates, int minimum = 1) => _resolve().EnumerateProgramDuelTargets(actor, candidates, minimum);
    internal bool HasProgramIronChainTargetExpansion(int sourceSeat) => _resolve().HasProgramIronChainTargetExpansion(sourceSeat);
    internal void ResolveProgramMultiTargetDuel(CharacterState source, Card duel, LegalAction action) => _resolve().ResolveProgramMultiTargetDuel(source, duel, action);
}
