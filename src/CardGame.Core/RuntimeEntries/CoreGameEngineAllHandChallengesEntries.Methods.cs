#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAllHandChallengesEntries
{
    internal int AllHandChallengeDamage(ProgramSkillFrame f, int target) => _resolve().AllHandChallengeDamage(f, target);
    internal bool AllHandChallengeQuotaAvailable(int owner, string skill, string binding) => _resolve().AllHandChallengeQuotaAvailable(owner, skill, binding);
    internal bool AllowsAllHandChallengeQuota(ProgramActivationLegalityContext gate) => _resolve().AllowsAllHandChallengeQuota(gate);
    internal SkillProgramStepOutcome PublishAllHandChallenge(ProgramSkillFrame f, SkillProgramEffect effect) => _resolve().PublishAllHandChallenge(f, effect);
    internal void ResolveAllHandChallengeChoice(ProgramSkillFrame f, PromptChoice choice) => _resolve().ResolveAllHandChallengeChoice(f, choice);
    internal SkillProgramStepOutcome RunAllHandChallenge(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().RunAllHandChallenge(supplied, effect);
    internal PromptChoice SelectAiAllHandChallenge(PendingDecision p, ProgramSkillFrame f, SkillProgramEffect effect) => _resolve().SelectAiAllHandChallenge(p, f, effect);
}
