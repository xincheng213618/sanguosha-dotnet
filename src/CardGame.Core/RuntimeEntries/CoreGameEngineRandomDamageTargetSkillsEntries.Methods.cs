#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRandomDamageTargetSkillsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome AcquireRandomDamageTargetSkill(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().AcquireRandomDamageTargetSkill(supplied);
    internal void AssertRandomDamageTargetSkill(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertRandomDamageTargetSkill(f);
    internal bool CanContinueRandomDamageTargetSkill(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueRandomDamageTargetSkill(f);
    internal bool CanOfferRandomDamageTargetSkill(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context, int ownerSeat) => _resolve().CanOfferRandomDamageTargetSkill(trigger, context, ownerSeat);
    internal global::System.Collections.Generic.HashSet<string> OwnedAndRestorableRandomSkillIds(global::CardGame.Core.CharacterState owner) => _resolve().OwnedAndRestorableRandomSkillIds(owner);
    internal global::CardGame.Core.ProgramSkillFrame? RandomDamageTargetSkillDamageCursorRoot(global::CardGame.Core.DamageTriggerWindowFrame window, scoped out long? dyingId) => _resolve().RandomDamageTargetSkillDamageCursorRoot(window, out dyingId);
    internal bool RandomDamageTargetSkillGrantExpired(global::CardGame.Core.SkillGrant grant) => _resolve().RandomDamageTargetSkillGrantExpired(grant);
    internal bool ResumeRandomDamageTargetSkill(long id) => _resolve().ResumeRandomDamageTargetSkill(id);
    internal bool TryGetRandomSkillDirectSlashUse(global::CardGame.Core.GameEngine.IDamageAttempt attack, scoped out global::CardGame.Core.CardUseFrame use) => _resolve().TryGetRandomSkillDirectSlashUse(attack, out use);
    internal bool ValidRandomDamageTargetSkillReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidRandomDamageTargetSkillReceipt(f);
}
