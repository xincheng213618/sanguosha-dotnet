#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundDistinctBasicPlayEntries
{
    internal IEnumerable<LegalAction> BuildRoundDistinctBasicPlayActions(CharacterState owner) => _resolve().BuildRoundDistinctBasicPlayActions(owner);
    internal bool CanUseRoundDistinctBasicAlcohol(CharacterState owner, IReadOnlyList<Card> cards) => _resolve().CanUseRoundDistinctBasicAlcohol(owner, cards);
    internal IReadOnlyList<int> RoundDistinctBasicPlayTargets(CharacterState owner, ProgramMultiCardViewAsSelection selection) => _resolve().RoundDistinctBasicPlayTargets(owner, selection);
    internal bool TryExecuteRoundDistinctBasicPlay(CharacterState owner, LegalAction selected, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => _resolve().TryExecuteRoundDistinctBasicPlay(owner, selected, cards, targets);
    internal bool TryFinishRoundDistinctBasicAlcoholCost(CardUseFrame use, Card card) => _resolve().TryFinishRoundDistinctBasicAlcoholCost(use, card);
    internal bool TryResolveRoundDistinctBasicDyingResponse(CharacterState owner, ProgramMultiCardViewAsSelection selection) => _resolve().TryResolveRoundDistinctBasicDyingResponse(owner, selection);
    internal bool TrySubmitRoundDistinctBasicPlay(UseProgramSkillCommand command, scoped out CommandResult result) => _resolve().TrySubmitRoundDistinctBasicPlay(command, out result);
    internal CommandError? ValidateRoundDistinctBasicPlaySelection(CharacterState owner, LegalAction action, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => _resolve().ValidateRoundDistinctBasicPlaySelection(owner, action, cards, targets);
}
