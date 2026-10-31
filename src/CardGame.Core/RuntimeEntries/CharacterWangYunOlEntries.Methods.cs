#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangYunOlEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome AwakenFromIssuedLianjiSlashDamage(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().AwakenFromIssuedLianjiSlashDamage(supplied);
    internal void BindWangYunNativeUse(long useId) => _resolve().BindWangYunNativeUse(useId);
    internal bool CanAwakenWangYun(int owner, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().CanAwakenWangYun(owner, trigger);
    internal bool CanContinueWangYun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueWangYun(f);
    internal bool CanWangYunLightning(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanWangYunLightning(f);
    internal void CompleteWangYunNativePayment(long useId) => _resolve().CompleteWangYunNativePayment(useId);
    internal void FinishWangYun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishWangYun(f);
    internal bool IsWangYunMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsWangYunMovement(f, e, p);
    internal bool IsWangYunNativeMaterialPayment(global::CardGame.Core.WangYunNativeUseOrigin origin, global::CardGame.Core.CardActionCost material, global::CardGame.Core.CardMovementRecord movement, long after) => _resolve().IsWangYunNativeMaterialPayment(origin, material, movement, after);
    internal bool IsWangYunRandomConversionAvailable(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource source, bool forResponse) => _resolve().IsWangYunRandomConversionAvailable(owner, card, source, forResponse);
    internal void IssueWangYunSlash(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice selected) => _resolve().IssueWangYunSlash(f, selected);
    internal void IssueWangYunTrick(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PromptChoice selected) => _resolve().IssueWangYunTrick(f, selected);
    internal void IssueWangYunWeapon(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueWangYunWeapon(f);
    internal void ObserveWangYunLianjiDamage(global::CardGame.Core.DamageAppliedEvent damage) => _resolve().ObserveWangYunLianjiDamage(damage);
    internal void PayWangYunPlanMovement(global::CardGame.Core.ProgramSkillFrame f, int card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, string next) => _resolve().PayWangYunPlanMovement(f, card, from, to, next);
    internal void PublishWangYun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishWangYun(f);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveLianjiPaidWeaponAndSlash(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveLianjiPaidWeaponAndSlash(supplied);
    internal void ResolveWangYunChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveWangYunChoice(selected);
    internal bool ResumeWangYun(long id) => _resolve().ResumeWangYun(id);
    internal void ResumeWangYunCompletedNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().ResumeWangYunCompletedNativeUse(use);
    internal bool ReturnWangYunMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnWangYunMovement(f);
    internal bool ReturnWangYunNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnWangYunNativeUse(use);
    internal bool TryAdvanceWangYunSubtree() => _resolve().TryAdvanceWangYunSubtree();
    internal global::CardGame.Core.SkillProgramStepOutcome UseThreeRandomTricksFromSelectedMaterial(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().UseThreeRandomTricksFromSelectedMaterial(supplied);
    internal bool ValidWangYun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidWangYun(f);
    internal bool ValidWangYunNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().ValidWangYunNativeUse(use);
    internal bool ValidWangYunRoundOnceHeader(global::CardGame.Core.RoundOnceCardlessBasicPaymentReceipt payment, global::CardGame.Core.CardActionContext action, int actor) => _resolve().ValidWangYunRoundOnceHeader(payment, action, actor);
    internal bool ValidWangYunRoundOnceStoredUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.WangYunNativeUseOrigin origin) => _resolve().ValidWangYunRoundOnceStoredUse(use, origin);
    internal bool ValidWangYunStoredOrigin(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.WangYunNativeUseOrigin? origin, bool returned, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> events) => _resolve().ValidWangYunStoredOrigin(f, origin, returned, events);
    internal void ValidateWangYunNativeAttackReturn(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ValidateWangYunNativeAttackReturn(completion);
    internal bool WangYunActivationCardAllowed(global::CardGame.Core.SkillProgramActivation a, global::CardGame.Core.Card card) => _resolve().WangYunActivationCardAllowed(a, card);
    internal bool WangYunActivationCardAllowed2(int ownerSeat, global::CardGame.Core.SkillProgramActivation a, global::CardGame.Core.Card card) => _resolve().WangYunActivationCardAllowed(ownerSeat, a, card);
    internal bool WangYunActivationMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().WangYunActivationMatches(f, op);
    internal bool WangYunCardUseRidesOn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent) => _resolve().WangYunCardUseRidesOn(use, parent);
    internal int WangYunChooser(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WangYunChooser(f);
    internal bool WangYunConversionUnavailable(global::CardGame.Core.ProgramViewAsSourceQuery query) => _resolve().WangYunConversionUnavailable(query);
    internal bool WangYunFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().WangYunFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? WangYunObserverRoot() => _resolve().WangYunObserverRoot();
    internal global::CardGame.Core.SkillProgramEffect? WangYunPausedEffect(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WangYunPausedEffect(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> WangYunPlanChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WangYunPlanChoices(f);
    internal global::CardGame.Core.CardKind[] WangYunRandomTrickPool() => _resolve().WangYunRandomTrickPool();
    internal global::CardGame.Core.CardConversionSource WangYunSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WangYunSource(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> WangYunTrickActions(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WangYunTrickActions(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> WangYunTrickChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().WangYunTrickChoices(f);
    internal bool HasWangYunPlanCapability { get => _resolve().HasWangYunPlanCapability; }
    internal bool HasWangYunTrickCapability { get => _resolve().HasWangYunTrickCapability; }
}
