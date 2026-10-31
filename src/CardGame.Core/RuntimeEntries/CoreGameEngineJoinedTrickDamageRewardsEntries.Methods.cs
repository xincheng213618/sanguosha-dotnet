#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJoinedTrickDamageRewardsEntries
{
    internal void AssertJoinedTrickDamageBenefits(CardUseFrame use) => _resolve().AssertJoinedTrickDamageBenefits(use);
    internal SkillProgramStepOutcome BeginJoinedTrickDamageReward(ProgramSkillFrame supplied) => _resolve().BeginJoinedTrickDamageReward(supplied);
    internal bool CanContinueIssuedJoinedTrickDamageReward(ProgramSkillFrame frame) => _resolve().CanContinueIssuedJoinedTrickDamageReward(frame);
    internal bool CanOfferJoinedTrickDamageReward(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferJoinedTrickDamageReward(candidate, trigger, context);
    internal void CollectIssuedJoinedTrickDamageRewardCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedJoinedTrickDamageRewardCandidates(action, window, result);
    internal JoinedTrickDamageBenefit? ExactIssuedJoinedTrickDamageRewardCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, bool paid) => _resolve().ExactIssuedJoinedTrickDamageRewardCandidate(candidate, context, paid);
    internal JoinedTrickDamageBenefit? ExactJoinedTrickDamageRewardParent(ProgramSkillFrame frame) => _resolve().ExactJoinedTrickDamageRewardParent(frame);
    internal bool HasIssuedJoinedTrickDamageRewardCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().HasIssuedJoinedTrickDamageRewardCandidate(candidate, context);
    internal bool HasJoinedTrickOwnerDamage(CardUseFrame use, int owner) => _resolve().HasJoinedTrickOwnerDamage(use, owner);
    internal bool ResumeJoinedTrickDamageReward(long id) => _resolve().ResumeJoinedTrickDamageReward(id);
    internal bool ReturnJoinedTrickDamageRewardMovement(ProgramSkillFrame frame) => _resolve().ReturnJoinedTrickDamageRewardMovement(frame);
    internal bool ValidJoinedTrickDamageReward(ProgramSkillFrame frame) => _resolve().ValidJoinedTrickDamageReward(frame);
    internal long JoinedTrickMovementSequence { get => _resolve().JoinedTrickMovementSequence; }
}
