#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangYunOlEntries
{
    internal SkillProgramStepOutcome AwakenFromIssuedLianjiSlashDamage(ProgramSkillFrame supplied) => _resolve().AwakenFromIssuedLianjiSlashDamage(supplied);
    internal void BindWangYunNativeUse(long useId) => _resolve().BindWangYunNativeUse(useId);
    internal bool CanAwakenWangYun(int owner, SkillProgramTrigger trigger) => _resolve().CanAwakenWangYun(owner, trigger);
    internal bool CanContinueWangYun(ProgramSkillFrame f) => _resolve().CanContinueWangYun(f);
    internal bool CanWangYunLightning(ProgramSkillFrame f) => _resolve().CanWangYunLightning(f);
    internal void CompleteWangYunNativePayment(long useId) => _resolve().CompleteWangYunNativePayment(useId);
    internal void FinishWangYun(ProgramSkillFrame f) => _resolve().FinishWangYun(f);
    internal bool IsWangYunMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().IsWangYunMovement(f, e, p);
    internal bool IsWangYunNativeMaterialPayment(WangYunNativeUseOrigin origin, CardActionCost material, CardMovementRecord movement, long after) => _resolve().IsWangYunNativeMaterialPayment(origin, material, movement, after);
    internal bool IsWangYunRandomConversionAvailable(CharacterState owner, Card card, CardConversionSource source, bool forResponse) => _resolve().IsWangYunRandomConversionAvailable(owner, card, source, forResponse);
    internal void IssueWangYunSlash(ProgramSkillFrame f, PromptChoice selected) => _resolve().IssueWangYunSlash(f, selected);
    internal void IssueWangYunTrick(ProgramSkillFrame f, PromptChoice selected) => _resolve().IssueWangYunTrick(f, selected);
    internal void IssueWangYunWeapon(ProgramSkillFrame f) => _resolve().IssueWangYunWeapon(f);
    internal void ObserveWangYunLianjiDamage(DamageAppliedEvent damage) => _resolve().ObserveWangYunLianjiDamage(damage);
    internal void PayWangYunPlanMovement(ProgramSkillFrame f, int card, CardLocation from, CardLocation to, string next) => _resolve().PayWangYunPlanMovement(f, card, from, to, next);
    internal void PublishWangYun(ProgramSkillFrame f) => _resolve().PublishWangYun(f);
    internal SkillProgramStepOutcome ResolveLianjiPaidWeaponAndSlash(ProgramSkillFrame supplied) => _resolve().ResolveLianjiPaidWeaponAndSlash(supplied);
    internal void ResolveWangYunChoice(PromptChoice selected) => _resolve().ResolveWangYunChoice(selected);
    internal bool ResumeWangYun(long id) => _resolve().ResumeWangYun(id);
    internal void ResumeWangYunCompletedNativeUse(CardUseFrame use) => _resolve().ResumeWangYunCompletedNativeUse(use);
    internal bool ReturnWangYunMovement(ProgramSkillFrame f) => _resolve().ReturnWangYunMovement(f);
    internal bool ReturnWangYunNativeUse(CardUseFrame use) => _resolve().ReturnWangYunNativeUse(use);
    internal bool TryAdvanceWangYunSubtree() => _resolve().TryAdvanceWangYunSubtree();
    internal SkillProgramStepOutcome UseThreeRandomTricksFromSelectedMaterial(ProgramSkillFrame supplied) => _resolve().UseThreeRandomTricksFromSelectedMaterial(supplied);
    internal bool ValidWangYun(ProgramSkillFrame f) => _resolve().ValidWangYun(f);
    internal bool ValidWangYunNativeUse(CardUseFrame use) => _resolve().ValidWangYunNativeUse(use);
    internal bool ValidWangYunRoundOnceHeader(RoundOnceCardlessBasicPaymentReceipt payment, CardActionContext action, int actor) => _resolve().ValidWangYunRoundOnceHeader(payment, action, actor);
    internal bool ValidWangYunRoundOnceStoredUse(CardUseFrame use, WangYunNativeUseOrigin origin) => _resolve().ValidWangYunRoundOnceStoredUse(use, origin);
    internal bool ValidWangYunStoredOrigin(ProgramSkillFrame f, WangYunNativeUseOrigin? origin, bool returned, IReadOnlyList<IGameEvent> events) => _resolve().ValidWangYunStoredOrigin(f, origin, returned, events);
    internal void ValidateWangYunNativeAttackReturn(GameEngine.AttackCompletionReceipt completion) => _resolve().ValidateWangYunNativeAttackReturn(completion);
    internal bool WangYunActivationCardAllowed(SkillProgramActivation a, Card card) => _resolve().WangYunActivationCardAllowed(a, card);
    internal bool WangYunActivationCardAllowed2(int ownerSeat, SkillProgramActivation a, Card card) => _resolve().WangYunActivationCardAllowed(ownerSeat, a, card);
    internal bool WangYunActivationMatches(ProgramSkillFrame f, SkillProgramEffectOp op) => _resolve().WangYunActivationMatches(f, op);
    internal bool WangYunCardUseRidesOn(CardUseFrame use, ProgramSkillFrame parent) => _resolve().WangYunCardUseRidesOn(use, parent);
    internal int WangYunChooser(ProgramSkillFrame f) => _resolve().WangYunChooser(f);
    internal bool WangYunConversionUnavailable(ProgramViewAsSourceQuery query) => _resolve().WangYunConversionUnavailable(query);
    internal bool WangYunFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().WangYunFirstChild(f, child);
    internal ProgramSkillFrame? WangYunObserverRoot() => _resolve().WangYunObserverRoot();
    internal SkillProgramEffect? WangYunPausedEffect(ProgramSkillFrame f) => _resolve().WangYunPausedEffect(f);
    internal IReadOnlyList<PromptChoice> WangYunPlanChoices(ProgramSkillFrame f) => _resolve().WangYunPlanChoices(f);
    internal CardKind[] WangYunRandomTrickPool() => _resolve().WangYunRandomTrickPool();
    internal CardConversionSource WangYunSource(ProgramSkillFrame f) => _resolve().WangYunSource(f);
    internal IReadOnlyList<LegalAction> WangYunTrickActions(ProgramSkillFrame f) => _resolve().WangYunTrickActions(f);
    internal IReadOnlyList<PromptChoice> WangYunTrickChoices(ProgramSkillFrame f) => _resolve().WangYunTrickChoices(f);
    internal bool HasWangYunPlanCapability { get => _resolve().HasWangYunPlanCapability; }
    internal bool HasWangYunTrickCapability { get => _resolve().HasWangYunTrickCapability; }
}
