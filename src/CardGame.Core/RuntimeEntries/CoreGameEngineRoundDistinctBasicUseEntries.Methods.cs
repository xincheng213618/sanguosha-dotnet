#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundDistinctBasicUseEntries
{
    internal void AssertRoundDistinctBasicDyingParent(CardUseFrame use, RoundDistinctBasicUseReceipt receipt, IReadOnlyList<IGameEvent> timeline, int acceptedIndex) => _resolve().AssertRoundDistinctBasicDyingParent(use, receipt, timeline, acceptedIndex);
    internal void AssertRoundDistinctBasicUses(CardUseFrame use) => _resolve().AssertRoundDistinctBasicUses(use);
    internal bool CanUseRoundDistinctBasicConversion(CharacterState owner, IndexedSkillProgramInstance instance, SkillProgramViewAs rule, bool forResponse, bool dyingUse = false) => _resolve().CanUseRoundDistinctBasicConversion(owner, instance, rule, forResponse, dyingUse);
    internal bool IsRoundDistinctBasicMaterial(CharacterState owner, Card card) => _resolve().IsRoundDistinctBasicMaterial(owner, card);
    internal void IssueRoundDistinctBasicUse(CardUseFrame use, CardActionContext action, bool response) => _resolve().IssueRoundDistinctBasicUse(use, action, response);
    internal bool MatchesRoundDistinctBasicMaterials(CharacterState owner, SkillProgramViewAs rule, IReadOnlyList<Card> cards) => _resolve().MatchesRoundDistinctBasicMaterials(owner, rule, cards);
    internal void ObserveRoundDistinctBasicUse(IGameEvent payload) => _resolve().ObserveRoundDistinctBasicUse(payload);
    internal bool RoundDistinctBasicNameUsed(int ownerSeat, string skillId, ProgramRoundDistinctBasicUsePolicy policy, CardKind outputKind) => _resolve().RoundDistinctBasicNameUsed(ownerSeat, skillId, policy, outputKind);
}
