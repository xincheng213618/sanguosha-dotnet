#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPangTongFriendEntries
{
    internal SkillProgramStepOutcome BeginFriendManjuan(ProgramSkillFrame frame) => _resolve().BeginFriendManjuan(frame);
    internal SkillProgramStepOutcome BeginFriendYangming(ProgramSkillFrame frame) => _resolve().BeginFriendYangming(frame);
    internal bool CanOfferFriendPangTong(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferFriendPangTong(candidate, trigger, context);
    internal void CleanupFriendYangming(ProgramSkillFrame frame, bool reward = true) => _resolve().CleanupFriendYangming(frame, reward);
    internal bool CompleteFriendYangmingNativeAttackReturn() => _resolve().CompleteFriendYangmingNativeAttackReturn();
    internal void ContinueFriendPangTong(ProgramSkillFrame frame) => _resolve().ContinueFriendPangTong(frame);
    internal void FinishFriendPangTong(ProgramSkillFrame frame) => _resolve().FinishFriendPangTong(frame);
    internal int FriendActualPlayHandLossCount(int seat) => _resolve().FriendActualPlayHandLossCount(seat);
    internal IReadOnlyList<int> FriendGainCardIds(ProgramSkillWindowContext context, int seat) => _resolve().FriendGainCardIds(context, seat);
    internal int FriendManjuanMovedCount(CardMovementBatchContext batch, int seat, SkillProgramTrigger trigger, int fallback) => _resolve().FriendManjuanMovedCount(batch, seat, trigger, fallback);
    internal IReadOnlyList<PromptChoice> FriendPangTongChoices(ProgramSkillFrame frame) => _resolve().FriendPangTongChoices(frame);
    internal void FriendPangTongMutation(ProgramSkillFrame frame, FriendPangTongStage next, Action mutation) => _resolve().FriendPangTongMutation(frame, next, mutation);
    internal IEnumerable<CardSnapshot> FriendPangTongPublicCards() => _resolve().FriendPangTongPublicCards();
    internal IReadOnlyList<LegalAction> FriendYangmingActions(ProgramSkillFrame frame) => _resolve().FriendYangmingActions(frame);
    internal bool HasFriendYangmingQuota(CharacterState owner) => _resolve().HasFriendYangmingQuota(owner);
    internal bool IsFriendYangmingOriginalUseChoice(CharacterState owner, Card card, CardKind kind) => _resolve().IsFriendYangmingOriginalUseChoice(owner, card, kind);
    internal bool IsFriendYangmingOriginalUseFrame(ProgramSkillFrame frame, CharacterState owner, Card card, CardKind kind) => _resolve().IsFriendYangmingOriginalUseFrame(frame, owner, card, kind);
    internal bool IsFriendYangmingOriginalUseParent(CardUseFrame use) => _resolve().IsFriendYangmingOriginalUseParent(use);
    internal void IssueFriendYangmingUse(ProgramSkillFrame frame, LegalAction action) => _resolve().IssueFriendYangmingUse(frame, action);
    internal void PublishFriendPangTongChoice(ProgramSkillFrame frame) => _resolve().PublishFriendPangTongChoice(frame);
    internal void ResolveFriendPangTongChoice(PromptChoice selected) => _resolve().ResolveFriendPangTongChoice(selected);
    internal bool ResumeFriendPangTong(long id) => _resolve().ResumeFriendPangTong(id);
    internal bool ReturnFriendPangTongMovement(ProgramSkillFrame frame) => _resolve().ReturnFriendPangTongMovement(frame);
    internal void ReturnFriendYangmingUse(CardUseFrame use) => _resolve().ReturnFriendYangmingUse(use);
}
