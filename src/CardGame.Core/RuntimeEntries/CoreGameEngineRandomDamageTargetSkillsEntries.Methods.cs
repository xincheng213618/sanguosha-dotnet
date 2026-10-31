#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRandomDamageTargetSkillsEntries
{
    internal SkillProgramStepOutcome AcquireRandomDamageTargetSkill(ProgramSkillFrame supplied) => _resolve().AcquireRandomDamageTargetSkill(supplied);
    internal bool CanContinueRandomDamageTargetSkill(ProgramSkillFrame f) => _resolve().CanContinueRandomDamageTargetSkill(f);
    internal bool CanOfferRandomDamageTargetSkill(SkillProgramTrigger trigger, ProgramSkillWindowContext context, int ownerSeat) => _resolve().CanOfferRandomDamageTargetSkill(trigger, context, ownerSeat);
    internal HashSet<string> OwnedAndRestorableRandomSkillIds(CharacterState owner) => _resolve().OwnedAndRestorableRandomSkillIds(owner);
    internal bool RandomDamageTargetSkillGrantExpired(SkillGrant grant) => _resolve().RandomDamageTargetSkillGrantExpired(grant);
    internal bool ResumeRandomDamageTargetSkill(long id) => _resolve().ResumeRandomDamageTargetSkill(id);
    internal bool TryGetRandomSkillDirectSlashUse(GameEngine.IDamageAttempt attack, scoped out CardUseFrame use) => _resolve().TryGetRandomSkillDirectSlashUse(attack, out use);
    internal bool ValidRandomDamageTargetSkillReceipt(ProgramSkillFrame f) => _resolve().ValidRandomDamageTargetSkillReceipt(f);
}
