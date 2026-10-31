#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashTargetBenefitObserversEntries
{
    internal bool IsSlashTargetBenefitProgramDying() => _resolve().IsSlashTargetBenefitProgramDying();
    internal bool SlashBenefitDiscardLedger(int cardId, CardLocation from, long before, long after, string reason) => _resolve().SlashBenefitDiscardLedger(cardId, from, before, after, reason);
    internal ProgramSkillFrame? SlashBenefitPaidObserverRoot(long? exactDamageWindow = default) => _resolve().SlashBenefitPaidObserverRoot(exactDamageWindow);
    internal bool SlashBenefitWindowMatches(SlashTargetBenefitWindowFrame w) => _resolve().SlashBenefitWindowMatches(w);
    internal bool ValidSlashBenefitDraftPayment(ProgramSkillFrame f) => _resolve().ValidSlashBenefitDraftPayment(f);
    internal bool ValidSlashTargetBenefitReceipt(SlashTargetBenefitReceipt r) => _resolve().ValidSlashTargetBenefitReceipt(r);
}
