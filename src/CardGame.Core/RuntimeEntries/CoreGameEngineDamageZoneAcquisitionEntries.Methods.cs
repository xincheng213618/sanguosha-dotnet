#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageZoneAcquisitionEntries
{
    internal void AssertRegisteredDamageAcquisitionReceipt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertRegisteredDamageAcquisitionReceipt(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDamageZoneAcquisition(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginDamageZoneAcquisition(input);
    internal bool CanContinueDamageZoneAcquisition(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueDamageZoneAcquisition(f);
    internal bool CanOfferDamageZoneAndAdjacentTarget(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferDamageZoneAndAdjacentTarget(candidate, trigger, context);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DamageAcquisitionChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DamageAcquisitionChoices(f);
    internal bool DamageAcquisitionHasExactOwningAttack(global::CardGame.Core.DamageFrame damage, global::CardGame.Core.DamageTriggerWindowFrame window) => _resolve().DamageAcquisitionHasExactOwningAttack(damage, window);
    internal global::CardGame.Core.CardConversionSource DamageAcquisitionSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DamageAcquisitionSource(f);
    internal bool DrainDamageAcquisitionChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainDamageAcquisitionChildren(f);
    internal bool ExactDamageAcquisitionParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExactDamageAcquisitionParent(f);
    internal bool IsDamageAcquisitionMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsDamageAcquisitionMovement(f, e, p);
    internal void IssueDamageZoneAcquisition(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueDamageZoneAcquisition(f);
    internal void PublishDamageAcquisitionChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishDamageAcquisitionChoice(f);
    internal void RequireDamageAcquisitionReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireDamageAcquisitionReceipt(f);
    internal void ResolveDamageAcquisitionChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveDamageAcquisitionChoice(selected);
    internal bool ResumeDamageAcquisition(long id) => _resolve().ResumeDamageAcquisition(id);
    internal bool ReturnDamageAcquisitionMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnDamageAcquisitionMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiDamageAcquisition(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiDamageAcquisition(p, f);
}
