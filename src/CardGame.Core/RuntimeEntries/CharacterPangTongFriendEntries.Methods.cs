#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPangTongFriendEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginFriendManjuan(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginFriendManjuan(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginFriendYangming(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginFriendYangming(frame);
    internal bool CanOfferFriendPangTong(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferFriendPangTong(candidate, trigger, context);
    internal void CleanupFriendYangming(global::CardGame.Core.ProgramSkillFrame frame, bool reward = true) => _resolve().CleanupFriendYangming(frame, reward);
    internal bool CompleteFriendYangmingNativeAttackReturn() => _resolve().CompleteFriendYangmingNativeAttackReturn();
    internal void ContinueFriendPangTong(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinueFriendPangTong(frame);
    internal void FinishFriendPangTong(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinishFriendPangTong(frame);
    internal int FriendActualPlayHandLossCount(int seat) => _resolve().FriendActualPlayHandLossCount(seat);
    internal global::System.Collections.Generic.IReadOnlyList<int> FriendGainCardIds(global::CardGame.Core.ProgramSkillWindowContext context, int seat) => _resolve().FriendGainCardIds(context, seat);
    internal int FriendManjuanMovedCount(global::CardGame.Core.CardMovementBatchContext batch, int seat, global::CardGame.Core.SkillProgramTrigger trigger, int fallback) => _resolve().FriendManjuanMovedCount(batch, seat, trigger, fallback);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> FriendPangTongChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FriendPangTongChoices(frame);
    internal void FriendPangTongMutation(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.FriendPangTongStage next, global::System.Action mutation) => _resolve().FriendPangTongMutation(frame, next, mutation);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> FriendPangTongPublicCards() => _resolve().FriendPangTongPublicCards();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> FriendYangmingActions(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FriendYangmingActions(frame);
    internal bool HasFriendYangmingQuota(global::CardGame.Core.CharacterState owner) => _resolve().HasFriendYangmingQuota(owner);
    internal bool IsFriendYangmingOriginalUseChoice(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind) => _resolve().IsFriendYangmingOriginalUseChoice(owner, card, kind);
    internal bool IsFriendYangmingOriginalUseFrame(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind) => _resolve().IsFriendYangmingOriginalUseFrame(frame, owner, card, kind);
    internal bool IsFriendYangmingOriginalUseParent(global::CardGame.Core.CardUseFrame use) => _resolve().IsFriendYangmingOriginalUseParent(use);
    internal void IssueFriendYangmingUse(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.LegalAction action) => _resolve().IssueFriendYangmingUse(frame, action);
    internal void PublishFriendPangTongChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishFriendPangTongChoice(frame);
    internal void ResolveFriendPangTongChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveFriendPangTongChoice(selected);
    internal bool ResumeFriendPangTong(long id) => _resolve().ResumeFriendPangTong(id);
    internal bool ReturnFriendPangTongMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnFriendPangTongMovement(frame);
    internal void ReturnFriendYangmingUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnFriendYangmingUse(use);
}
