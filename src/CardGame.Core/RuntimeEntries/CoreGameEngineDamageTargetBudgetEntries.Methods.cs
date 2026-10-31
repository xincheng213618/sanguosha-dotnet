#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageTargetBudgetEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome ApplyDamageTargetBudget(global::CardGame.Core.SkillProgramEffect e, global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ApplyDamageTargetBudget(e, supplied);
    internal void AssertDamageTargetBudgetReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertDamageTargetBudgetReceipt(f);
    internal global::CardGame.Core.CardConversionSource BudgetSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BudgetSource(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BuildDamageBudgetChoices(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DamageTargetBudgetReceipt r) => _resolve().BuildDamageBudgetChoices(f, use, r);
    internal bool CanOfferDamageTargetBudget(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferDamageTargetBudget(c, t, context);
    internal global::CardGame.Core.SkillProgramEffect? DamageBudgetSourceEffect(global::CardGame.Core.DamageTargetBudgetReceipt receipt) => _resolve().DamageBudgetSourceEffect(receipt);
    internal global::CardGame.Core.SkillProgramStepOutcome ExposeDamageBudgetChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExposeDamageBudgetChoice(f);
    internal bool HasDamageTargetBudgetTail(global::CardGame.Core.CardUseFrame use) => _resolve().HasDamageTargetBudgetTail(use);
    internal bool IsDamageBudgetResolvedFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DamageTargetBudgetResolvedEvent fact, global::System.Collections.Generic.IReadOnlyList<int> before, int? currentProofActor = default) => _resolve().IsDamageBudgetResolvedFact(use, fact, before, currentProofActor);
    internal global::CardGame.Core.CardUseFrame RequireDamageBudgetUse(global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().RequireDamageBudgetUse(f, out window);
    internal void ResolveDamageBudgetChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveDamageBudgetChoice(choice);
    internal global::CardGame.Core.PromptChoice SelectAiDamageBudgetChoice(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiDamageBudgetChoice(p, f);
    internal void SettleDamageMarkerBudgetTargets(long id) => _resolve().SettleDamageMarkerBudgetTargets(id);
    internal bool TracksDamageTargetBudgetTargets { get => _resolve().TracksDamageTargetBudgetTargets; }
}
