#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageTargetSkillRoundDrawEntries
{
    internal void AssertDamageTargetSkillRoundDraw(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertDamageTargetSkillRoundDraw(frame);
    internal bool CanOfferDamageTargetRoundSkill(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context, int ownerSeat) => _resolve().CanOfferDamageTargetRoundSkill(trigger, context, ownerSeat);
    internal bool ContinuePaidDamageTargetRoundSkill(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinuePaidDamageTargetRoundSkill(frame);
    internal global::CardGame.Core.ProgramSkillFrame? DamageTargetSkillRoundDamageCursorRoot(global::CardGame.Core.DamageTriggerWindowFrame window, scoped out long? dyingId) => _resolve().DamageTargetSkillRoundDamageCursorRoot(window, out dyingId);
    internal global::CardGame.Core.SkillProgramStepOutcome GrantDamageTargetSkillAndDrawByRound(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().GrantDamageTargetSkillAndDrawByRound(supplied);
    internal bool IsDamageTargetSkillRoundMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsDamageTargetSkillRoundMovement(f, e, p);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.SkillGrant> OwnedAndRestorableRoundSkillGrants(global::CardGame.Core.CharacterState owner, string skillId) => _resolve().OwnedAndRestorableRoundSkillGrants(owner, skillId);
    internal void RequireDamageTargetSkillRoundDraw(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().RequireDamageTargetSkillRoundDraw(frame);
    internal bool ResumeDamageTargetSkillRoundDraw(long id) => _resolve().ResumeDamageTargetSkillRoundDraw(id);
    internal void ReturnDamageTargetSkillRoundDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnDamageTargetSkillRoundDraw(supplied, outcome);
    internal bool ReturnDamageTargetSkillRoundMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnDamageTargetSkillRoundMovement(frame);
    internal bool ValidDamageTargetSkillRoundDraw(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidDamageTargetSkillRoundDraw(frame);
}
