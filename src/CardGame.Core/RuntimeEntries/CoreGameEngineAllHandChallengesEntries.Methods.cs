#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAllHandChallengesEntries
{
    internal int AllHandChallengeDamage(global::CardGame.Core.ProgramSkillFrame f, int target) => _resolve().AllHandChallengeDamage(f, target);
    internal bool AllHandChallengeQuotaAvailable(int owner, string skill, string binding) => _resolve().AllHandChallengeQuotaAvailable(owner, skill, binding);
    internal bool AllowsAllHandChallengeQuota(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().AllowsAllHandChallengeQuota(gate);
    internal global::CardGame.Core.SkillProgramStepOutcome PublishAllHandChallenge(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect effect) => _resolve().PublishAllHandChallenge(f, effect);
    internal void ResolveAllHandChallengeChoice(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice choice) => _resolve().ResolveAllHandChallengeChoice(f, choice);
    internal global::CardGame.Core.SkillProgramStepOutcome RunAllHandChallenge(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().RunAllHandChallenge(supplied, effect);
    internal global::CardGame.Core.PromptChoice SelectAiAllHandChallenge(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect effect) => _resolve().SelectAiAllHandChallenge(p, f, effect);
}
