#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageTargetSkillRoundDrawEntries
{
    internal bool CanOfferDamageTargetRoundSkill(SkillProgramTrigger trigger, ProgramSkillWindowContext context, int ownerSeat) => _resolve().CanOfferDamageTargetRoundSkill(trigger, context, ownerSeat);
    internal bool ContinuePaidDamageTargetRoundSkill(ProgramSkillFrame frame) => _resolve().ContinuePaidDamageTargetRoundSkill(frame);
    internal SkillProgramStepOutcome GrantDamageTargetSkillAndDrawByRound(ProgramSkillFrame supplied) => _resolve().GrantDamageTargetSkillAndDrawByRound(supplied);
    internal IEnumerable<SkillGrant> OwnedAndRestorableRoundSkillGrants(CharacterState owner, string skillId) => _resolve().OwnedAndRestorableRoundSkillGrants(owner, skillId);
    internal void RequireDamageTargetSkillRoundDraw(ProgramSkillFrame frame) => _resolve().RequireDamageTargetSkillRoundDraw(frame);
    internal bool ResumeDamageTargetSkillRoundDraw(long id) => _resolve().ResumeDamageTargetSkillRoundDraw(id);
    internal void ReturnDamageTargetSkillRoundDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnDamageTargetSkillRoundDraw(supplied, outcome);
    internal bool ReturnDamageTargetSkillRoundMovement(ProgramSkillFrame frame) => _resolve().ReturnDamageTargetSkillRoundMovement(frame);
    internal bool ValidDamageTargetSkillRoundDraw(ProgramSkillFrame frame) => _resolve().ValidDamageTargetSkillRoundDraw(frame);
}
