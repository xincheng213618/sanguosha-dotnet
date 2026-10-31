#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGiftedSlashUseRewardsEntries
{
    internal GiftedSlashHandPolicy? ActiveGiftedSlashPolicy(int recipient, int cardId) => _resolve().ActiveGiftedSlashPolicy(recipient, cardId);
    internal void AssertGiftedSlashPolicyOrigin(GiftedSlashHandPolicy policy) => _resolve().AssertGiftedSlashPolicyOrigin(policy);
    internal void AssertGiftedSlashUseBenefit(CardUseFrame use) => _resolve().AssertGiftedSlashUseBenefit(use);
    internal SkillProgramStepOutcome BeginGiftedSlashGift(SkillProgramEffect effect, ProgramSkillFrame input, int recipient) => _resolve().BeginGiftedSlashGift(effect, input, recipient);
    internal SkillProgramStepOutcome BeginGiftedSlashUseReward(SkillProgramEffect effect, ProgramSkillFrame input) => _resolve().BeginGiftedSlashUseReward(effect, input);
    internal bool CanContinueIssuedGiftedSlashReward(ProgramSkillFrame frame) => _resolve().CanContinueIssuedGiftedSlashReward(frame);
    internal bool CanOfferGiftedSlashUseReward(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferGiftedSlashUseReward(candidate, trigger, context);
    internal void CollectIssuedGiftedSlashRewardCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedGiftedSlashRewardCandidates(action, window, result);
    internal bool DrainGiftedSlashChildren(ProgramSkillFrame frame) => _resolve().DrainGiftedSlashChildren(frame);
    internal bool ExactGiftedSlashGiftActivation(ProgramSkillFrame frame) => _resolve().ExactGiftedSlashGiftActivation(frame);
    internal bool ExactGiftedSlashRewardParent(ProgramSkillFrame frame) => _resolve().ExactGiftedSlashRewardParent(frame);
    internal bool ExactIssuedGiftedSlashRewardCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, bool paid) => _resolve().ExactIssuedGiftedSlashRewardCandidate(candidate, context, paid);
    internal GameEngine.ProgramCardIdentityMatch? GetGiftedSlashIdentityMatch(CharacterState owner, Card card) => _resolve().GetGiftedSlashIdentityMatch(owner, card);
    internal bool GiftedSlashFirstChild(ProgramSkillFrame frame, ResolutionFrame child) => _resolve().GiftedSlashFirstChild(frame, child);
    internal ProgramSkillFrame? GiftedSlashObserverRoot() => _resolve().GiftedSlashObserverRoot();
    internal bool GiftedSlashPendingMatches(ProgramSkillFrame frame) => _resolve().GiftedSlashPendingMatches(frame);
    internal bool HasIssuedGiftedSlashRewardCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().HasIssuedGiftedSlashRewardCandidate(candidate, context);
    internal bool IsGiftedSlashProgramDying() => _resolve().IsGiftedSlashProgramDying();
    internal void IssueGiftedSlashUseBenefit(long useId, CardActionContext? action) => _resolve().IssueGiftedSlashUseBenefit(useId, action);
    internal bool ResumeGiftedSlashUseReward(long id) => _resolve().ResumeGiftedSlashUseReward(id);
    internal bool ReturnGiftedSlashUseRewardMovement(ProgramSkillFrame frame) => _resolve().ReturnGiftedSlashUseRewardMovement(frame);
    internal bool ValidGiftedSlashGift(ProgramSkillFrame frame) => _resolve().ValidGiftedSlashGift(frame);
    internal bool ValidGiftedSlashReward(ProgramSkillFrame frame) => _resolve().ValidGiftedSlashReward(frame);
    internal long GiftedSlashSequence { get => _resolve().GiftedSlashSequence; }
    internal bool TracksGiftedSlash { get => _resolve().TracksGiftedSlash; }
}
