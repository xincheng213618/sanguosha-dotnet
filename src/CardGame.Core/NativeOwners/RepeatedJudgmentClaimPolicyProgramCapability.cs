#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RepeatedJudgmentClaimPolicyProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineExactRepeatedJudgmentRuntime>> getCoreGameEngineExactRepeatedJudgmentRuntime) : IRepeatedJudgmentClaimPolicyProgramHost
{
    public SkillProgramStepOutcome RepeatJudgmentWithClaimPolicy(ProgramSkillFrame frame, string reason, string bind, IReadOnlyList<Suit> suits, SkillProgramClaimHandLimitExemption policy) => getCoreGameEngineExactRepeatedJudgmentRuntime().Value.BeginProgramExactRepeatedJudgment(frame, reason, bind, suits, policy);
}
