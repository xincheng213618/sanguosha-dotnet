#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashTargetBenefitObserversEntries
{
    internal bool HasSlashBenefitDamageObserver(long id) => _resolve().HasSlashBenefitDamageObserver(id);
    internal bool IsSlashTargetBenefitMovement(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.SkillProgramEffect? op, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsSlashTargetBenefitMovement(root, op, pending);
    internal bool IsSlashTargetBenefitProgramDying() => _resolve().IsSlashTargetBenefitProgramDying();
    internal bool SlashBenefitDiscardLedger(int cardId, global::CardGame.Core.CardLocation from, long before, long after, string reason) => _resolve().SlashBenefitDiscardLedger(cardId, from, before, after, reason);
    internal global::CardGame.Core.ProgramSkillFrame? SlashBenefitPaidObserverRoot(long? exactDamageWindow = default) => _resolve().SlashBenefitPaidObserverRoot(exactDamageWindow);
    internal bool SlashBenefitWindowMatches(global::CardGame.Core.SlashTargetBenefitWindowFrame w) => _resolve().SlashBenefitWindowMatches(w);
    internal bool ValidSlashBenefitDraftPayment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidSlashBenefitDraftPayment(f);
    internal bool ValidSlashTargetBenefitReceipt(global::CardGame.Core.SlashTargetBenefitReceipt r) => _resolve().ValidSlashTargetBenefitReceipt(r);
}
