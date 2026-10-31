#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuJinMouMobileEntries
{
    internal bool AdvanceMouYuJinChildren() => _resolve().AdvanceMouYuJinChildren();
    internal void AnswerMouJieyueGift(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerMouJieyueGift(selected);
    internal bool CanOfferMouJieyue(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanOfferMouJieyue(c);
    internal bool CanOfferMouXiayuan(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanOfferMouXiayuan(c);
    internal void CompleteMouJieyue(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CompleteMouJieyue(f);
    internal void ContinueMouJieyue(long id) => _resolve().ContinueMouJieyue(id);
    internal (global::CardGame.Core.ProgramAiEstimate Estimate, int? TargetSeat) EstimateMouXiayuanForAi(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramAiPublicContext context, global::CardGame.Core.ProgramSkillWindowContext? window) => _resolve().EstimateMouXiayuanForAi(owner, context, window);
    internal bool IsMouYuJinDying() => _resolve().IsMouYuJinDying();
    internal bool IsMouYuJinRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsMouYuJinRecoveryParent(f, recovery);
    internal bool KeepPaidMouYuJin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidMouYuJin(f);
    internal bool MouJieyueArmorGranted(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouJieyueArmorGranted(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouJieyueGiftChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouJieyueGiftChoices(f);
    internal bool MouJieyueRoot(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouJieyueRoot(f);
    internal global::CardGame.Core.DamageFrame? MouXiayuanDamage(global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().MouXiayuanDamage(c);
    internal global::CardGame.Core.Card[] MouXiayuanLegalCosts(int owner) => _resolve().MouXiayuanLegalCosts(owner);
    internal global::CardGame.Core.NativeLogicalDiscardInvocationRecordedEvent? MouXiayuanPayment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouXiayuanPayment(f);
    internal global::CardGame.Core.DamageFrame? MouXiayuanRoot(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouXiayuanRoot(f);
    internal bool MouYuJinFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouYuJinFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? MouYuJinObserverRoot() => _resolve().MouYuJinObserverRoot();
    internal bool MouYuJinObserverSuffix(int index) => _resolve().MouYuJinObserverSuffix(index);
    internal bool MouYuJinPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouYuJinPaid(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveMouJieyue(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveMouJieyue(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveMouXiayuan(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveMouXiayuan(supplied);
    internal bool ResumeMouYuJin(long id) => _resolve().ResumeMouYuJin(id);
    internal bool ReturnMouJieyueMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouJieyueMovement(f);
    internal bool ReturnMouXiayuanMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouXiayuanMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiMouJieyueGift(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiMouJieyueGift(p, f);
    internal global::CardGame.Core.PromptChoice SelectAiMouXiayuanCost(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiMouXiayuanCost(p, f);
    internal bool ValidMouJieyue(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouJieyue(f);
    internal bool ValidMouXiayuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouXiayuan(f);
    internal bool HasMouYuJinCapability { get => _resolve().HasMouYuJinCapability; }
}
