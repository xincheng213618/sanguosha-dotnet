#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJoinedTrickDamageRewardsEntries
{
    internal void AssertJoinedTrickDamageBenefits(global::CardGame.Core.CardUseFrame use) => _resolve().AssertJoinedTrickDamageBenefits(use);
    internal void AssertJoinedTrickDamageReward(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertJoinedTrickDamageReward(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginJoinedTrickDamageReward(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginJoinedTrickDamageReward(supplied);
    internal bool CanContinueIssuedJoinedTrickDamageReward(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinueIssuedJoinedTrickDamageReward(frame);
    internal bool CanOfferJoinedTrickDamageReward(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferJoinedTrickDamageReward(candidate, trigger, context);
    internal void CollectIssuedJoinedTrickDamageRewardCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.ProgramCardTriggerCandidate> result) => _resolve().CollectIssuedJoinedTrickDamageRewardCandidates(action, window, result);
    internal global::CardGame.Core.JoinedTrickDamageBenefit? ExactIssuedJoinedTrickDamageRewardCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, bool paid) => _resolve().ExactIssuedJoinedTrickDamageRewardCandidate(candidate, context, paid);
    internal global::CardGame.Core.JoinedTrickDamageBenefit? ExactJoinedTrickDamageRewardParent(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ExactJoinedTrickDamageRewardParent(frame);
    internal bool HasIssuedJoinedTrickDamageRewardCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().HasIssuedJoinedTrickDamageRewardCandidate(candidate, context);
    internal bool HasJoinedTrickOwnerDamage(global::CardGame.Core.CardUseFrame use, int owner) => _resolve().HasJoinedTrickOwnerDamage(use, owner);
    internal bool IsJoinedTrickDamageRewardAwaitedMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsJoinedTrickDamageRewardAwaitedMovement(frame, effect, pending);
    internal bool ResumeJoinedTrickDamageReward(long id) => _resolve().ResumeJoinedTrickDamageReward(id);
    internal bool ReturnJoinedTrickDamageRewardMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnJoinedTrickDamageRewardMovement(frame);
    internal bool ValidJoinedTrickDamageReward(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidJoinedTrickDamageReward(frame);
    internal long JoinedTrickMovementSequence { get => _resolve().JoinedTrickMovementSequence; }
}
