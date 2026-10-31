#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramIronChainTargetsEntries
{
    internal void AddProgramIronChainUseActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> candidates, global::CardGame.Core.CardKind? playedCardKind = default, global::CardGame.Core.CardConversionSource? conversionSource = default) => _resolve().AddProgramIronChainUseActions(actions, actor, physicalCard, candidates, playedCardKind, conversionSource);
    internal global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.IReadOnlyList<int>> EnumerateProgramIronChainTargets(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> candidates) => _resolve().EnumerateProgramIronChainTargets(actor, candidates);
    internal int GetProgramIronChainTargetLimit(global::CardGame.Core.CharacterState actor) => _resolve().GetProgramIronChainTargetLimit(actor);
}
