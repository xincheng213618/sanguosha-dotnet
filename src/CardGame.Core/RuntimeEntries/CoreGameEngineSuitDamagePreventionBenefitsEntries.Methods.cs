#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSuitDamagePreventionBenefitsEntries
{
    internal SkillProgramStepOutcome BeginSuitPreventionBenefit(ProgramSkillFrame input) => _resolve().BeginSuitPreventionBenefit(input);
    internal bool CanOfferSuitPreventionBenefit(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferSuitPreventionBenefit(candidate, trigger, context);
    internal void CaptureSuitPreventionDamageCompleted(ProgramSkillFrame f, bool applied) => _resolve().CaptureSuitPreventionDamageCompleted(f, applied);
    internal bool DrainSuitPlacementChildren(ProgramSkillFrame f) => _resolve().DrainSuitPlacementChildren(f);
    internal bool IsValidSuitPreventionBenefit(ProgramSkillFrame f, bool requirePayment) => _resolve().IsValidSuitPreventionBenefit(f, requirePayment);
    internal void PublishSuitPreventionBenefit(ProgramSkillFrame f) => _resolve().PublishSuitPreventionBenefit(f);
    internal void ResolveSuitPreventionBenefit(PromptChoice choice) => _resolve().ResolveSuitPreventionBenefit(choice);
    internal bool ResumeSuitPreventionBenefit(long id) => _resolve().ResumeSuitPreventionBenefit(id);
    internal bool ReturnSuitPlacementMovement(ProgramSkillFrame f) => _resolve().ReturnSuitPlacementMovement(f);
    internal PromptChoice SelectAiSuitPreventionBenefit(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiSuitPreventionBenefit(decision, f);
    internal IReadOnlyList<Card> SuitBenefitCosts(int ownerSeat, string skillId, string instance, IReadOnlyList<Suit>? suits = default) => _resolve().SuitBenefitCosts(ownerSeat, skillId, instance, suits);
    internal SkillProgramEffect SuitBenefitEffect(ProgramSkillFrame f) => _resolve().SuitBenefitEffect(f);
    internal IReadOnlyList<PromptChoice> SuitPreventionBenefitChoices(ProgramSkillFrame f) => _resolve().SuitPreventionBenefitChoices(f);
}
