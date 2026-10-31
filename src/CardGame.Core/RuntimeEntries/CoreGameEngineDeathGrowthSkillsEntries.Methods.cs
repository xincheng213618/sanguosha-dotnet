#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeathGrowthSkillsEntries
{
    internal void AdvanceDeathGrowthChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AdvanceDeathGrowthChoice(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDeathGrowthSkill(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BeginDeathGrowthSkill(input, effect);
    internal bool CanContinueDeathGrowthSkill(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueDeathGrowthSkill(f);
    internal bool CanOfferDeathGrowthSkill(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferDeathGrowthSkill(candidate, trigger, context);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DeathGrowthChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DeathGrowthChoices(f);
    internal global::CardGame.Core.CardConversionSource DeathGrowthSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DeathGrowthSource(f);
    internal bool DrainDeathGrowthChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainDeathGrowthChildren(f);
    internal void FinishDeathGrowthSkill(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishDeathGrowthSkill(f);
    internal bool IsDeathGrowthMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsDeathGrowthMovement(f, effect, p);
    internal void ResolveDeathGrowthChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveDeathGrowthChoice(selected);
    internal bool ResumeDeathGrowthSkill(long id) => _resolve().ResumeDeathGrowthSkill(id);
    internal bool ReturnDeathGrowthMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnDeathGrowthMovement(f);
}
