#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageZoneAcquisitionEntries
{
    internal SkillProgramStepOutcome BeginDamageZoneAcquisition(ProgramSkillFrame input) => _resolve().BeginDamageZoneAcquisition(input);
    internal bool CanContinueDamageZoneAcquisition(ProgramSkillFrame f) => _resolve().CanContinueDamageZoneAcquisition(f);
    internal bool CanOfferDamageZoneAndAdjacentTarget(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferDamageZoneAndAdjacentTarget(candidate, trigger, context);
    internal IReadOnlyList<PromptChoice> DamageAcquisitionChoices(ProgramSkillFrame f) => _resolve().DamageAcquisitionChoices(f);
    internal bool DamageAcquisitionHasExactOwningAttack(DamageFrame damage, DamageTriggerWindowFrame window) => _resolve().DamageAcquisitionHasExactOwningAttack(damage, window);
    internal CardConversionSource DamageAcquisitionSource(ProgramSkillFrame f) => _resolve().DamageAcquisitionSource(f);
    internal bool DrainDamageAcquisitionChildren(ProgramSkillFrame f) => _resolve().DrainDamageAcquisitionChildren(f);
    internal bool ExactDamageAcquisitionParent(ProgramSkillFrame f) => _resolve().ExactDamageAcquisitionParent(f);
    internal void IssueDamageZoneAcquisition(ProgramSkillFrame f) => _resolve().IssueDamageZoneAcquisition(f);
    internal void PublishDamageAcquisitionChoice(ProgramSkillFrame f) => _resolve().PublishDamageAcquisitionChoice(f);
    internal void RequireDamageAcquisitionReceipt(ProgramSkillFrame f) => _resolve().RequireDamageAcquisitionReceipt(f);
    internal void ResolveDamageAcquisitionChoice(PromptChoice selected) => _resolve().ResolveDamageAcquisitionChoice(selected);
    internal bool ResumeDamageAcquisition(long id) => _resolve().ResumeDamageAcquisition(id);
    internal bool ReturnDamageAcquisitionMovement(ProgramSkillFrame f) => _resolve().ReturnDamageAcquisitionMovement(f);
    internal PromptChoice SelectAiDamageAcquisition(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectAiDamageAcquisition(p, f);
}
