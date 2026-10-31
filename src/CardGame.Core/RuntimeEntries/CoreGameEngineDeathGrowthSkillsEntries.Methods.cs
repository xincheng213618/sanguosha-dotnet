#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeathGrowthSkillsEntries
{
    internal void AdvanceDeathGrowthChoice(ProgramSkillFrame f) => _resolve().AdvanceDeathGrowthChoice(f);
    internal SkillProgramStepOutcome BeginDeathGrowthSkill(ProgramSkillFrame input, SkillProgramEffect effect) => _resolve().BeginDeathGrowthSkill(input, effect);
    internal bool CanContinueDeathGrowthSkill(ProgramSkillFrame f) => _resolve().CanContinueDeathGrowthSkill(f);
    internal bool CanOfferDeathGrowthSkill(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferDeathGrowthSkill(candidate, trigger, context);
    internal IReadOnlyList<PromptChoice> DeathGrowthChoices(ProgramSkillFrame f) => _resolve().DeathGrowthChoices(f);
    internal CardConversionSource DeathGrowthSource(ProgramSkillFrame f) => _resolve().DeathGrowthSource(f);
    internal bool DrainDeathGrowthChildren(ProgramSkillFrame f) => _resolve().DrainDeathGrowthChildren(f);
    internal void FinishDeathGrowthSkill(ProgramSkillFrame f) => _resolve().FinishDeathGrowthSkill(f);
    internal void ResolveDeathGrowthChoice(PromptChoice selected) => _resolve().ResolveDeathGrowthChoice(selected);
    internal bool ResumeDeathGrowthSkill(long id) => _resolve().ResumeDeathGrowthSkill(id);
    internal bool ReturnDeathGrowthMovement(ProgramSkillFrame f) => _resolve().ReturnDeathGrowthMovement(f);
}
