#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5LiJueEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShanheS5Langxi(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginShanheS5Langxi(supplied);
    internal bool CanOfferShanheS5Langxi(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferShanheS5Langxi(candidate, trigger, context);
    internal global::CardGame.Core.PromptChoice SelectAiShanheS5Langxi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiShanheS5Langxi(decision, frame);
    internal int ShanheS5LangxiMinimumDamage(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ShanheS5LangxiMinimumDamage(frame);
}
