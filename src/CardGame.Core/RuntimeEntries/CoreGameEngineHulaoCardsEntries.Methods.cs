#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHulaoCardsEntries
{
    internal void AddHulaoAllianceFeastActions(List<LegalAction> actions, CharacterState actor, IEnumerable<Card> playableCards) => _resolve().AddHulaoAllianceFeastActions(actions, actor, playableCards);
    internal void CompleteHulaoAllianceFeastTarget(long useId) => _resolve().CompleteHulaoAllianceFeastTarget(useId);
    internal bool HasHulaoAllianceFeastPrompt() => _resolve().HasHulaoAllianceFeastPrompt();
    internal void ResolveAllianceFeast(CharacterState source, Card card) => _resolve().ResolveAllianceFeast(source, card);
    internal void ResolveAllianceFeastEffect(NullificationWindowFrame pending) => _resolve().ResolveAllianceFeastEffect(pending);
    internal void ResolveHulaoAllianceFeastChoice(PromptChoice selected) => _resolve().ResolveHulaoAllianceFeastChoice(selected);
    internal bool TryAdvanceHulaoAllianceFeastAi() => _resolve().TryAdvanceHulaoAllianceFeastAi();
}
