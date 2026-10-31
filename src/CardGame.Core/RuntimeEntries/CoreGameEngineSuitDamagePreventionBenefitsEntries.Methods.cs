#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSuitDamagePreventionBenefitsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginSuitPreventionBenefit(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginSuitPreventionBenefit(input);
    internal bool CanOfferSuitPreventionBenefit(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferSuitPreventionBenefit(candidate, trigger, context);
    internal void CaptureSuitPreventionDamageCompleted(global::CardGame.Core.ProgramSkillFrame f, bool applied) => _resolve().CaptureSuitPreventionDamageCompleted(f, applied);
    internal bool DrainSuitPlacementChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainSuitPlacementChildren(f);
    internal bool IsValidSuitPreventionBenefit(global::CardGame.Core.ProgramSkillFrame f, bool requirePayment) => _resolve().IsValidSuitPreventionBenefit(f, requirePayment);
    internal void PublishSuitPreventionBenefit(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishSuitPreventionBenefit(f);
    internal void ResolveSuitPreventionBenefit(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveSuitPreventionBenefit(choice);
    internal bool ResumeSuitPreventionBenefit(long id) => _resolve().ResumeSuitPreventionBenefit(id);
    internal bool ReturnSuitPlacementMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnSuitPlacementMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiSuitPreventionBenefit(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiSuitPreventionBenefit(decision, f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> SuitBenefitCosts(int ownerSeat, string skillId, string instance, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Suit>? suits = default) => _resolve().SuitBenefitCosts(ownerSeat, skillId, instance, suits);
    internal global::CardGame.Core.SkillProgramEffect SuitBenefitEffect(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SuitBenefitEffect(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SuitPreventionBenefitChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SuitPreventionBenefitChoices(f);
}
