#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageTargetBudgetEntries
{
    internal SkillProgramStepOutcome ApplyDamageTargetBudget(SkillProgramEffect e, ProgramSkillFrame supplied) => _resolve().ApplyDamageTargetBudget(e, supplied);
    internal CardConversionSource BudgetSource(ProgramSkillFrame f) => _resolve().BudgetSource(f);
    internal IReadOnlyList<PromptChoice> BuildDamageBudgetChoices(ProgramSkillFrame f, CardUseFrame use, DamageTargetBudgetReceipt r) => _resolve().BuildDamageBudgetChoices(f, use, r);
    internal bool CanOfferDamageTargetBudget(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().CanOfferDamageTargetBudget(c, t, context);
    internal SkillProgramEffect? DamageBudgetSourceEffect(DamageTargetBudgetReceipt receipt) => _resolve().DamageBudgetSourceEffect(receipt);
    internal SkillProgramStepOutcome ExposeDamageBudgetChoice(ProgramSkillFrame f) => _resolve().ExposeDamageBudgetChoice(f);
    internal bool HasDamageTargetBudgetTail(CardUseFrame use) => _resolve().HasDamageTargetBudgetTail(use);
    internal bool IsDamageBudgetResolvedFact(CardUseFrame use, DamageTargetBudgetResolvedEvent fact, IReadOnlyList<int> before, int? currentProofActor = default) => _resolve().IsDamageBudgetResolvedFact(use, fact, before, currentProofActor);
    internal CardUseFrame RequireDamageBudgetUse(ProgramSkillFrame f, scoped out ProgramCardTriggerWindowFrame window) => _resolve().RequireDamageBudgetUse(f, out window);
    internal void ResolveDamageBudgetChoice(PromptChoice choice) => _resolve().ResolveDamageBudgetChoice(choice);
    internal PromptChoice SelectAiDamageBudgetChoice(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectAiDamageBudgetChoice(p, f);
    internal void SettleDamageMarkerBudgetTargets(long id) => _resolve().SettleDamageMarkerBudgetTargets(id);
    internal bool TracksDamageTargetBudgetTargets { get => _resolve().TracksDamageTargetBudgetTargets; }
}
