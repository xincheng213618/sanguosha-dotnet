#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiangWeiMobileGodEntries
{
    internal bool AdmitMobileJwDeath(ProgramTriggerLegalityContext c) => _resolve().AdmitMobileJwDeath(c);
    internal bool AdvanceMobileJwSubtree() => _resolve().AdvanceMobileJwSubtree();
    internal void AnswerMobileJw(PromptChoice selected) => _resolve().AnswerMobileJw(selected);
    internal SkillProgramStepOutcome BeginMobileJw(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginMobileJw(supplied, op);
    internal void BindMobileJwXinghunSlashUse(long useId, long? parentId) => _resolve().BindMobileJwXinghunSlashUse(useId, parentId);
    internal PromptChoice ChooseMobileJw(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseMobileJw(p, f);
    internal void ContinueMobileJw(long id) => _resolve().ContinueMobileJw(id);
    internal void DiscardMobileJw(ProgramSkillFrame f, IReadOnlyList<int> ids) => _resolve().DiscardMobileJw(f, ids);
    internal bool DrainMobileJw(ProgramSkillFrame f) => _resolve().DrainMobileJw(f);
    internal void FinishMobileJw(ProgramSkillFrame f) => _resolve().FinishMobileJw(f);
    internal bool IsMobileJwShenpeiDamage(ProgramSkillFrame f, int target, int amount, ProgramParticipantReference? source, DamageNature? nature, bool sourceLess, int? explicitSource) => _resolve().IsMobileJwShenpeiDamage(f, target, amount, source, nature, sourceLess, explicitSource);
    internal bool IsMobileJwSuspendedDying(long attackId) => _resolve().IsMobileJwSuspendedDying(attackId);
    internal bool IsMobileJwXinghunSlashSource(long? parentId, int actor, int target, Card card, CardKind kind, int physicalOwner, CardLocation from, IReadOnlyList<Card>? materials, bool countsTowardLimit) => _resolve().IsMobileJwXinghunSlashSource(parentId, actor, target, card, kind, physicalOwner, from, materials, countsTowardLimit);
    internal bool IsOriginalDyingSuspendedByMobileJwShenpei(DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByMobileJwShenpei(dying);
    internal bool KeepPaidMobileJw(ProgramSkillFrame f) => _resolve().KeepPaidMobileJw(f);
    internal IReadOnlyList<PromptChoice> MobileJwChoices(ProgramSkillFrame f) => _resolve().MobileJwChoices(f);
    internal IReadOnlyList<Card> MobileJwDiscardable(ProgramSkillFrame f, int seat, CardZoneKind zone) => _resolve().MobileJwDiscardable(f, seat, zone);
    internal bool MobileJwDyingProof() => _resolve().MobileJwDyingProof();
    internal bool MobileJwFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MobileJwFirstChild(f, child);
    internal bool MobileJwHuitianUsed(int owner) => _resolve().MobileJwHuitianUsed(owner);
    internal bool MobileJwMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().MobileJwMovement(f, effect, p);
    internal ProgramSkillFrame? MobileJwObserverRoot() => _resolve().MobileJwObserverRoot();
    internal bool MobileJwParent(ProgramSkillFrame f) => _resolve().MobileJwParent(f);
    internal bool MobileJwRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MobileJwRecovery(f, recovery);
    internal void MutateMobileJw(ProgramSkillFrame f, MobileJiangWeiStage stage, MobileJiangWeiStage next, Action mutation) => _resolve().MutateMobileJw(f, stage, next, mutation);
    internal void NextMobileJwSlash(ProgramSkillFrame f) => _resolve().NextMobileJwSlash(f);
    internal bool PayMobileJwReservedSlash(IReadOnlyList<Card> cards, int provider, CardLocation? source, CardMoveReason reason) => _resolve().PayMobileJwReservedSlash(cards, provider, source, reason);
    internal void PublishMobileJw(ProgramSkillFrame f) => _resolve().PublishMobileJw(f);
    internal void RecordMobileJwPayment(long id) => _resolve().RecordMobileJwPayment(id);
    internal bool ResumeMobileJw(long id) => _resolve().ResumeMobileJw(id);
    internal bool ReturnMobileJwAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().ReturnMobileJwAttack(completion);
    internal bool ReturnMobileJwMovement(ProgramSkillFrame f) => _resolve().ReturnMobileJwMovement(f);
    internal void ReturnMobileJwShownTop(ProgramSkillFrame f) => _resolve().ReturnMobileJwShownTop(f);
    internal void ReturnMobileJwUse(CardUseFrame use) => _resolve().ReturnMobileJwUse(use);
    internal void ValidateMobileJwUse(CardUseFrame use) => _resolve().ValidateMobileJwUse(use);
}
