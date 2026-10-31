#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHulaoCardsEntries
{
    internal void AddHulaoAllianceFeastActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> playableCards) => _resolve().AddHulaoAllianceFeastActions(actions, actor, playableCards);
    internal void CompleteHulaoAllianceFeastTarget(long useId) => _resolve().CompleteHulaoAllianceFeastTarget(useId);
    internal bool HasHulaoAllianceFeastPrompt() => _resolve().HasHulaoAllianceFeastPrompt();
    internal void ResolveAllianceFeast(global::CardGame.Core.CharacterState source, global::CardGame.Core.Card card) => _resolve().ResolveAllianceFeast(source, card);
    internal void ResolveAllianceFeastEffect(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().ResolveAllianceFeastEffect(pending);
    internal void ResolveHulaoAllianceFeastChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveHulaoAllianceFeastChoice(selected);
    internal bool TryAdvanceHulaoAllianceFeastAi() => _resolve().TryAdvanceHulaoAllianceFeastAi();
}
