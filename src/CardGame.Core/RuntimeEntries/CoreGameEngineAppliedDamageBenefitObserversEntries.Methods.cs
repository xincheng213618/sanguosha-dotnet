#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAppliedDamageBenefitObserversEntries
{
    internal bool AppliedDamageBenefitFirstBatch(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ProgramAppliedDamageBenefit receipt, global::CardGame.Core.CardsMovedTriggerWindowFrame movement) => _resolve().AppliedDamageBenefitFirstBatch(root, receipt, movement);
    internal global::CardGame.Core.ProgramSkillFrame? AppliedDamageBenefitRoot(long damageWindowId) => _resolve().AppliedDamageBenefitRoot(damageWindowId);
    internal bool AppliedDamageFieldPayment(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ProgramAppliedDamageBenefit receipt) => _resolve().AppliedDamageFieldPayment(root, receipt);
    internal void AssertAppliedDamageBenefitReceipt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertAppliedDamageBenefitReceipt(frame);
    internal bool HasAppliedDamageBenefitObserver(long damageWindowId) => _resolve().HasAppliedDamageBenefitObserver(damageWindowId);
    internal bool IsAppliedDamageBenefitDying() => _resolve().IsAppliedDamageBenefitDying();
}
