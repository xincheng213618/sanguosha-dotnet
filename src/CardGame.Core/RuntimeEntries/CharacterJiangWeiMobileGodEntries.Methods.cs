#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiangWeiMobileGodEntries
{
    internal bool AdmitMobileJwDeath(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().AdmitMobileJwDeath(c);
    internal bool AdvanceMobileJwSubtree() => _resolve().AdvanceMobileJwSubtree();
    internal void AnswerMobileJw(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerMobileJw(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileJw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginMobileJw(supplied, op);
    internal void BindMobileJwXinghunSlashUse(long useId, long? parentId) => _resolve().BindMobileJwXinghunSlashUse(useId, parentId);
    internal global::CardGame.Core.PromptChoice ChooseMobileJw(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseMobileJw(p, f);
    internal void ContinueMobileJw(long id) => _resolve().ContinueMobileJw(id);
    internal void DiscardMobileJw(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<int> ids) => _resolve().DiscardMobileJw(f, ids);
    internal bool DrainMobileJw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMobileJw(f);
    internal void FinishMobileJw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMobileJw(f);
    internal bool IsMobileJwShenpeiDamage(global::CardGame.Core.ProgramSkillFrame f, int target, int amount, global::CardGame.Core.ProgramParticipantReference? source, global::CardGame.Core.DamageNature? nature, bool sourceLess, int? explicitSource) => _resolve().IsMobileJwShenpeiDamage(f, target, amount, source, nature, sourceLess, explicitSource);
    internal bool IsMobileJwSuspendedDying(long attackId) => _resolve().IsMobileJwSuspendedDying(attackId);
    internal bool IsMobileJwXinghunSlashSource(long? parentId, int actor, int target, global::CardGame.Core.Card card, global::CardGame.Core.CardKind kind, int physicalOwner, global::CardGame.Core.CardLocation from, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? materials, bool countsTowardLimit) => _resolve().IsMobileJwXinghunSlashSource(parentId, actor, target, card, kind, physicalOwner, from, materials, countsTowardLimit);
    internal bool IsOriginalDyingSuspendedByMobileJwShenpei(global::CardGame.Core.DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByMobileJwShenpei(dying);
    internal bool KeepPaidMobileJw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidMobileJw(f);
    internal bool MobileJwBeforeDamageObserver(long id) => _resolve().MobileJwBeforeDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MobileJwChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileJwChoices(f);
    internal bool MobileJwDamageObserver(long id) => _resolve().MobileJwDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> MobileJwDiscardable(global::CardGame.Core.ProgramSkillFrame f, int seat, global::CardGame.Core.CardZoneKind zone) => _resolve().MobileJwDiscardable(f, seat, zone);
    internal bool MobileJwDyingProof() => _resolve().MobileJwDyingProof();
    internal bool MobileJwFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MobileJwFirstChild(f, child);
    internal bool MobileJwHuitianUsed(int owner) => _resolve().MobileJwHuitianUsed(owner);
    internal bool MobileJwMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MobileJwMovement(f, effect, p);
    internal global::CardGame.Core.ProgramSkillFrame? MobileJwObserverRoot() => _resolve().MobileJwObserverRoot();
    internal bool MobileJwParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileJwParent(f);
    internal bool MobileJwRecovery(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MobileJwRecovery(f, recovery);
    internal void MutateMobileJw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.MobileJiangWeiStage stage, global::CardGame.Core.MobileJiangWeiStage next, global::System.Action mutation) => _resolve().MutateMobileJw(f, stage, next, mutation);
    internal void NextMobileJwSlash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().NextMobileJwSlash(f);
    internal bool PayMobileJwReservedSlash(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, int provider, global::CardGame.Core.CardLocation? source, global::CardGame.Core.CardMoveReason reason) => _resolve().PayMobileJwReservedSlash(cards, provider, source, reason);
    internal void PublishMobileJw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMobileJw(f);
    internal void RecordMobileJwPayment(long id) => _resolve().RecordMobileJwPayment(id);
    internal bool ResumeMobileJw(long id) => _resolve().ResumeMobileJw(id);
    internal bool ReturnMobileJwAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnMobileJwAttack(completion);
    internal bool ReturnMobileJwMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMobileJwMovement(f);
    internal void ReturnMobileJwShownTop(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMobileJwShownTop(f);
    internal void ReturnMobileJwUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnMobileJwUse(use);
    internal void ValidateMobileJwUse(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateMobileJwUse(use);
}
