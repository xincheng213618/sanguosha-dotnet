#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RepeatedJudgmentClaimPolicyProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineExactRepeatedJudgmentEntries> getCoreGameEngineExactRepeatedJudgmentRuntime) : IRepeatedJudgmentClaimPolicyProgramHost
{
    public SkillProgramStepOutcome RepeatJudgmentWithClaimPolicy(ProgramSkillFrame frame, string reason, string bind, IReadOnlyList<Suit> suits, SkillProgramClaimHandLimitExemption policy) => getCoreGameEngineExactRepeatedJudgmentRuntime().BeginProgramExactRepeatedJudgment(frame, reason, bind, suits, policy);
}
