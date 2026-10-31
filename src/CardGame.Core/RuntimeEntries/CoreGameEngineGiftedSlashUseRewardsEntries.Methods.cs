#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGiftedSlashUseRewardsEntries
{
    internal global::CardGame.Core.GiftedSlashHandPolicy? ActiveGiftedSlashPolicy(int recipient, int cardId) => _resolve().ActiveGiftedSlashPolicy(recipient, cardId);
    internal void AssertGiftedSlashPolicyOrigin(global::CardGame.Core.GiftedSlashHandPolicy policy) => _resolve().AssertGiftedSlashPolicyOrigin(policy);
    internal void AssertGiftedSlashUseBenefit(global::CardGame.Core.CardUseFrame use) => _resolve().AssertGiftedSlashUseBenefit(use);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginGiftedSlashGift(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame input, int recipient) => _resolve().BeginGiftedSlashGift(effect, input, recipient);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginGiftedSlashUseReward(global::CardGame.Core.SkillProgramEffect effect, global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginGiftedSlashUseReward(effect, input);
    internal bool CanContinueIssuedGiftedSlashReward(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinueIssuedGiftedSlashReward(frame);
    internal bool CanOfferGiftedSlashUseReward(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferGiftedSlashUseReward(candidate, trigger, context);
    internal void CollectIssuedGiftedSlashRewardCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedGiftedSlashRewardCandidates(action, window, result);
    internal bool DrainGiftedSlashChildren(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DrainGiftedSlashChildren(frame);
    internal bool ExactGiftedSlashGiftActivation(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ExactGiftedSlashGiftActivation(frame);
    internal bool ExactGiftedSlashRewardParent(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ExactGiftedSlashRewardParent(frame);
    internal bool ExactIssuedGiftedSlashRewardCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, bool paid) => _resolve().ExactIssuedGiftedSlashRewardCandidate(candidate, context, paid);
    internal global::CardGame.Core.GameEngine.ProgramCardIdentityMatch? GetGiftedSlashIdentityMatch(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().GetGiftedSlashIdentityMatch(owner, card);
    internal bool GiftedSlashFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => _resolve().GiftedSlashFirstChild(frame, child);
    internal global::CardGame.Core.ProgramSkillFrame? GiftedSlashObserverRoot() => _resolve().GiftedSlashObserverRoot();
    internal bool GiftedSlashPendingMatches(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().GiftedSlashPendingMatches(frame);
    internal bool HasGiftedSlashDamageObserver(long id) => _resolve().HasGiftedSlashDamageObserver(id);
    internal bool HasIssuedGiftedSlashRewardCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().HasIssuedGiftedSlashRewardCandidate(candidate, context);
    internal bool IsGiftedSlashAwaitedMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsGiftedSlashAwaitedMovement(frame, effect, pending);
    internal bool IsGiftedSlashProgramDying() => _resolve().IsGiftedSlashProgramDying();
    internal void IssueGiftedSlashUseBenefit(long useId, global::CardGame.Core.CardActionContext? action) => _resolve().IssueGiftedSlashUseBenefit(useId, action);
    internal bool ResumeGiftedSlashUseReward(long id) => _resolve().ResumeGiftedSlashUseReward(id);
    internal bool ReturnGiftedSlashUseRewardMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnGiftedSlashUseRewardMovement(frame);
    internal bool ValidGiftedSlashGift(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidGiftedSlashGift(frame);
    internal bool ValidGiftedSlashReward(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidGiftedSlashReward(frame);
    internal long GiftedSlashSequence { get => _resolve().GiftedSlashSequence; }
    internal bool TracksGiftedSlash { get => _resolve().TracksGiftedSlash; }
}
