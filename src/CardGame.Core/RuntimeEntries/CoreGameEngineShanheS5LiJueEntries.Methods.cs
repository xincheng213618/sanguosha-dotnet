#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5LiJueEntries
{
    internal SkillProgramStepOutcome BeginShanheS5Langxi(ProgramSkillFrame supplied) => _resolve().BeginShanheS5Langxi(supplied);
    internal bool CanOfferShanheS5Langxi(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferShanheS5Langxi(candidate, trigger, context);
    internal PromptChoice SelectAiShanheS5Langxi(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiShanheS5Langxi(decision, frame);
    internal int ShanheS5LangxiMinimumDamage(ProgramSkillFrame frame) => _resolve().ShanheS5LangxiMinimumDamage(frame);
}
