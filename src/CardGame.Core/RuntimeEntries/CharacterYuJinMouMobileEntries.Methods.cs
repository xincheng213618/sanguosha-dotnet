#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuJinMouMobileEntries
{
    internal bool AdvanceMouYuJinChildren() => _resolve().AdvanceMouYuJinChildren();
    internal void AnswerMouJieyueGift(PromptChoice selected) => _resolve().AnswerMouJieyueGift(selected);
    internal bool CanOfferMouJieyue(ProgramTriggerLegalityContext c) => _resolve().CanOfferMouJieyue(c);
    internal bool CanOfferMouXiayuan(ProgramTriggerLegalityContext c) => _resolve().CanOfferMouXiayuan(c);
    internal void CompleteMouJieyue(ProgramSkillFrame f) => _resolve().CompleteMouJieyue(f);
    internal void ContinueMouJieyue(long id) => _resolve().ContinueMouJieyue(id);
    internal (ProgramAiEstimate Estimate, int? TargetSeat) EstimateMouXiayuanForAi(CharacterState owner, ProgramAiPublicContext context, ProgramSkillWindowContext? window) => _resolve().EstimateMouXiayuanForAi(owner, context, window);
    internal bool IsMouYuJinDying() => _resolve().IsMouYuJinDying();
    internal bool IsMouYuJinRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsMouYuJinRecoveryParent(f, recovery);
    internal bool KeepPaidMouYuJin(ProgramSkillFrame f) => _resolve().KeepPaidMouYuJin(f);
    internal bool MouJieyueArmorGranted(ProgramSkillFrame f) => _resolve().MouJieyueArmorGranted(f);
    internal IReadOnlyList<PromptChoice> MouJieyueGiftChoices(ProgramSkillFrame f) => _resolve().MouJieyueGiftChoices(f);
    internal bool MouJieyueRoot(ProgramSkillFrame f) => _resolve().MouJieyueRoot(f);
    internal DamageFrame? MouXiayuanDamage(ProgramSkillWindowContext c) => _resolve().MouXiayuanDamage(c);
    internal Card[] MouXiayuanLegalCosts(int owner) => _resolve().MouXiayuanLegalCosts(owner);
    internal NativeLogicalDiscardInvocationRecordedEvent? MouXiayuanPayment(ProgramSkillFrame f) => _resolve().MouXiayuanPayment(f);
    internal DamageFrame? MouXiayuanRoot(ProgramSkillFrame f) => _resolve().MouXiayuanRoot(f);
    internal bool MouYuJinFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouYuJinFirstChild(f, child);
    internal ProgramSkillFrame? MouYuJinObserverRoot() => _resolve().MouYuJinObserverRoot();
    internal bool MouYuJinObserverSuffix(int index) => _resolve().MouYuJinObserverSuffix(index);
    internal bool MouYuJinPaid(ProgramSkillFrame f) => _resolve().MouYuJinPaid(f);
    internal SkillProgramStepOutcome ResolveMouJieyue(ProgramSkillFrame supplied) => _resolve().ResolveMouJieyue(supplied);
    internal SkillProgramStepOutcome ResolveMouXiayuan(ProgramSkillFrame supplied) => _resolve().ResolveMouXiayuan(supplied);
    internal bool ResumeMouYuJin(long id) => _resolve().ResumeMouYuJin(id);
    internal bool ReturnMouJieyueMovement(ProgramSkillFrame f) => _resolve().ReturnMouJieyueMovement(f);
    internal bool ReturnMouXiayuanMovement(ProgramSkillFrame f) => _resolve().ReturnMouXiayuanMovement(f);
    internal PromptChoice SelectAiMouJieyueGift(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectAiMouJieyueGift(p, f);
    internal PromptChoice SelectAiMouXiayuanCost(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectAiMouXiayuanCost(p, f);
    internal bool ValidMouJieyue(ProgramSkillFrame f) => _resolve().ValidMouJieyue(f);
    internal bool ValidMouXiayuan(ProgramSkillFrame f) => _resolve().ValidMouXiayuan(f);
    internal bool HasMouYuJinCapability { get => _resolve().HasMouYuJinCapability; }
}
