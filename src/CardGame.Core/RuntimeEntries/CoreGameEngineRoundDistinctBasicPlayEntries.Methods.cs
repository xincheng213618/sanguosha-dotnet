#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundDistinctBasicPlayEntries
{
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.LegalAction> BuildRoundDistinctBasicPlayActions(global::CardGame.Core.CharacterState owner) => _resolve().BuildRoundDistinctBasicPlayActions(owner);
    internal bool CanUseRoundDistinctBasicAlcohol(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().CanUseRoundDistinctBasicAlcohol(owner, cards);
    internal global::System.Collections.Generic.IReadOnlyList<int> RoundDistinctBasicPlayTargets(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => _resolve().RoundDistinctBasicPlayTargets(owner, selection);
    internal bool TryExecuteRoundDistinctBasicPlay(global::CardGame.Core.CharacterState owner, global::CardGame.Core.LegalAction selected, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().TryExecuteRoundDistinctBasicPlay(owner, selected, cards, targets);
    internal bool TryFinishRoundDistinctBasicAlcoholCost(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.Card card) => _resolve().TryFinishRoundDistinctBasicAlcoholCost(use, card);
    internal bool TryResolveRoundDistinctBasicDyingResponse(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => _resolve().TryResolveRoundDistinctBasicDyingResponse(owner, selection);
    internal bool TrySubmitRoundDistinctBasicPlay(global::CardGame.Core.UseProgramSkillCommand command, scoped out global::CardGame.Core.CommandResult result) => _resolve().TrySubmitRoundDistinctBasicPlay(command, out result);
    internal global::CardGame.Core.CommandError? ValidateRoundDistinctBasicPlaySelection(global::CardGame.Core.CharacterState owner, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int> cards, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().ValidateRoundDistinctBasicPlaySelection(owner, action, cards, targets);
}
