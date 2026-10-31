#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAlcoholSlashSkillReplacementsEntries
{
    internal global::CardGame.Core.ProgramSkillFrame? AlcoholSlashReplacementDamageCursorRoot(global::CardGame.Core.DamageTriggerWindowFrame window, scoped out long? dyingId) => _resolve().AlcoholSlashReplacementDamageCursorRoot(window, out dyingId);
    internal void AssertAlcoholSlashSkillReplacement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertAlcoholSlashSkillReplacement(f);
    internal bool CanContinueAlcoholSlashSkillReplacement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueAlcoholSlashSkillReplacement(f);
    internal bool CanOfferAlcoholSlashSkillReplacement(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context, int ownerSeat) => _resolve().CanOfferAlcoholSlashSkillReplacement(trigger, context, ownerSeat);
    internal global::CardGame.Core.SkillProgramStepOutcome ReplaceOwnSkillsAfterAlcoholSlashDamage(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ReplaceOwnSkillsAfterAlcoholSlashDamage(supplied);
    internal bool ResumeAlcoholSlashSkillReplacement(long id) => _resolve().ResumeAlcoholSlashSkillReplacement(id);
    internal bool TryGetPermanentAlcoholSlashUse(global::CardGame.Core.GameEngine.IDamageAttempt attack, scoped out global::CardGame.Core.CardUseFrame use) => _resolve().TryGetPermanentAlcoholSlashUse(attack, out use);
    internal bool ValidAlcoholSlashSkillReplacementReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidAlcoholSlashSkillReplacementReceipt(f);
    internal bool WasGrantPermanentlyRevokedAfterLoss(int ownerSeat, global::CardGame.Core.SkillGrant grant, global::CardGame.Core.IGameEvent originalLoss) => _resolve().WasGrantPermanentlyRevokedAfterLoss(ownerSeat, grant, originalLoss);
}
