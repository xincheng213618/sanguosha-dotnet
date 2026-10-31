#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAlcoholSlashSkillReplacementsEntries
{
    internal bool CanContinueAlcoholSlashSkillReplacement(ProgramSkillFrame f) => _resolve().CanContinueAlcoholSlashSkillReplacement(f);
    internal bool CanOfferAlcoholSlashSkillReplacement(SkillProgramTrigger trigger, ProgramSkillWindowContext context, int ownerSeat) => _resolve().CanOfferAlcoholSlashSkillReplacement(trigger, context, ownerSeat);
    internal SkillProgramStepOutcome ReplaceOwnSkillsAfterAlcoholSlashDamage(ProgramSkillFrame supplied) => _resolve().ReplaceOwnSkillsAfterAlcoholSlashDamage(supplied);
    internal bool ResumeAlcoholSlashSkillReplacement(long id) => _resolve().ResumeAlcoholSlashSkillReplacement(id);
    internal bool TryGetPermanentAlcoholSlashUse(GameEngine.IDamageAttempt attack, scoped out CardUseFrame use) => _resolve().TryGetPermanentAlcoholSlashUse(attack, out use);
    internal bool ValidAlcoholSlashSkillReplacementReceipt(ProgramSkillFrame f) => _resolve().ValidAlcoholSlashSkillReplacementReceipt(f);
    internal bool WasGrantPermanentlyRevokedAfterLoss(int ownerSeat, SkillGrant grant, IGameEvent originalLoss) => _resolve().WasGrantPermanentlyRevokedAfterLoss(ownerSeat, grant, originalLoss);
}
