#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramIronChainTargetsEntries
{
    internal void AddProgramIronChainUseActions(ICollection<LegalAction> actions, CharacterState actor, Card physicalCard, IReadOnlyList<CharacterState> candidates, CardKind? playedCardKind = default, CardConversionSource? conversionSource = default) => _resolve().AddProgramIronChainUseActions(actions, actor, physicalCard, candidates, playedCardKind, conversionSource);
    internal IEnumerable<IReadOnlyList<int>> EnumerateProgramIronChainTargets(CharacterState actor, IReadOnlyList<CharacterState> candidates) => _resolve().EnumerateProgramIronChainTargets(actor, candidates);
    internal int GetProgramIronChainTargetLimit(CharacterState actor) => _resolve().GetProgramIronChainTargetLimit(actor);
}
