#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundDistinctBasicUseEntries
{
    internal void AssertRoundDistinctBasicDyingParent(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.RoundDistinctBasicUseReceipt receipt, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> timeline, int acceptedIndex) => _resolve().AssertRoundDistinctBasicDyingParent(use, receipt, timeline, acceptedIndex);
    internal void AssertRoundDistinctBasicUses(global::CardGame.Core.CardUseFrame use) => _resolve().AssertRoundDistinctBasicUses(use);
    internal bool CanUseRoundDistinctBasicConversion(global::CardGame.Core.CharacterState owner, global::CardGame.Core.IndexedSkillProgramInstance instance, global::CardGame.Core.SkillProgramViewAs rule, bool forResponse, bool dyingUse = false) => _resolve().CanUseRoundDistinctBasicConversion(owner, instance, rule, forResponse, dyingUse);
    internal bool IsRoundDistinctBasicMaterial(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().IsRoundDistinctBasicMaterial(owner, card);
    internal void IssueRoundDistinctBasicUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext action, bool response) => _resolve().IssueRoundDistinctBasicUse(use, action, response);
    internal bool MatchesRoundDistinctBasicMaterials(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramViewAs rule, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().MatchesRoundDistinctBasicMaterials(owner, rule, cards);
    internal void ObserveRoundDistinctBasicUse(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveRoundDistinctBasicUse(payload);
    internal bool RoundDistinctBasicNameUsed(int ownerSeat, string skillId, global::CardGame.Core.ProgramRoundDistinctBasicUsePolicy policy, global::CardGame.Core.CardKind outputKind) => _resolve().RoundDistinctBasicNameUsed(ownerSeat, skillId, policy, outputKind);
}
