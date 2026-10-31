#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOriginalUseLightningEntries
{
    internal IReadOnlyList<PlayerMarkerSnapshot>? AppendOriginalUseLightningMarker(int seat, IReadOnlyList<PlayerMarkerSnapshot>? markers) => _resolve().AppendOriginalUseLightningMarker(seat, markers);
    internal OriginalUseLightningAttachment AttachOriginalUseLightning(CardUseFrame use, CardConversionSource source, string gameplayHash, int target) => _resolve().AttachOriginalUseLightning(use, source, gameplayHash, target);
    internal void BeginOriginalUseLightning(long useId, CardConversionSource source, string gameplayHash) => _resolve().BeginOriginalUseLightning(useId, source, gameplayHash);
    internal bool BeginOriginalUseLightningFinalized(long id, Card card, LegalActionKind actionKind) => _resolve().BeginOriginalUseLightningFinalized(id, card, actionKind);
    internal void BeginOriginalUseLightningJudgment(DelayedEffectContinuation delayed) => _resolve().BeginOriginalUseLightningJudgment(delayed);
    internal ProgramSkillFrame BindNativeDelayedAttackChild(ProgramSkillFrame child, long judgmentId) => _resolve().BindNativeDelayedAttackChild(child, judgmentId);
    internal void ClearUnavailableOriginalUseLightning() => _resolve().ClearUnavailableOriginalUseLightning();
    internal void CompleteOriginalUseLightningJudgment(JudgmentFrame pending, bool hit) => _resolve().CompleteOriginalUseLightningJudgment(pending, hit);
    internal void CompleteOriginalUseLightningNullification(DelayedEffectContinuation delayed, bool nullified, long windowId) => _resolve().CompleteOriginalUseLightningNullification(delayed, nullified, windowId);
    internal void ContinueOriginalUseLightningFinalized(ProgramCardTriggerWindowFrame window) => _resolve().ContinueOriginalUseLightningFinalized(window);
    internal bool ExactOriginalUseLightning(OriginalUseLightningAttachment a) => _resolve().ExactOriginalUseLightning(a);
    internal bool FinishOriginalUseLightningAttack(NativeOwners.CardAttackHandle attack) => _resolve().FinishOriginalUseLightningAttack(attack);
    internal bool HasHeldOriginalUseLightning(CharacterState player) => _resolve().HasHeldOriginalUseLightning(player);
    internal OriginalUseLightningAttachment? HeldOriginalUseLightning(long id) => _resolve().HeldOriginalUseLightning(id);
    internal bool IsOriginalUseLightning(long id) => _resolve().IsOriginalUseLightning(id);
    internal bool IsOriginalUseLightningFinalizedWindow(ProgramCardTriggerWindowFrame window) => _resolve().IsOriginalUseLightningFinalizedWindow(window);
    internal bool IsOriginalUseLightningJudgment(JudgmentFrame j) => _resolve().IsOriginalUseLightningJudgment(j);
    internal bool MatchesOriginalUseLightningAction(long id, CardActionContext action, int cardId) => _resolve().MatchesOriginalUseLightningAction(id, action, cardId);
    internal IReadOnlyList<int> OriginalUseLightningPlacementTargets(NativeOwners.JizhiResolution pending) => _resolve().OriginalUseLightningPlacementTargets(pending);
    internal bool PlaceOriginalUseLightning(NullificationWindowFrame pending) => _resolve().PlaceOriginalUseLightning(pending);
    internal void PreserveNativeDelayedAttackCompletion(GameEngine.AttackCompletionReceipt completion) => _resolve().PreserveNativeDelayedAttackCompletion(completion);
    internal void RemoveOriginalUseLightning(OriginalUseLightningAttachment a, string reason) => _resolve().RemoveOriginalUseLightning(a, reason);
    internal void ReplaceHeldOriginalUseLightning(OriginalUseLightningAttachment a) => _resolve().ReplaceHeldOriginalUseLightning(a);
    internal void ResumeNativeDelayedAttackCompletion(long judgmentId) => _resolve().ResumeNativeDelayedAttackCompletion(judgmentId);
    internal bool SkipUnavailableOriginalUseLightning(OriginalUseLightningAttachment a, int? excludedCard) => _resolve().SkipUnavailableOriginalUseLightning(a, excludedCard);
    internal void TransferOriginalUseLightning(OriginalUseLightningAttachment a, long judgmentId, int? cardId, int? excludedCard) => _resolve().TransferOriginalUseLightning(a, judgmentId, cardId, excludedCard);
    internal bool TryBeginHeldOriginalUseLightning(CharacterState current, int? excludedCard) => _resolve().TryBeginHeldOriginalUseLightning(current, excludedCard);
    internal bool TryContinueOriginalUseLightningPlacement(long id) => _resolve().TryContinueOriginalUseLightningPlacement(id);
    internal bool ValidOriginalUseLightning(OriginalUseLightningAttachment a) => _resolve().ValidOriginalUseLightning(a);
    internal bool ValidOriginalUseLightningDelayed(DelayedEffectContinuation d) => _resolve().ValidOriginalUseLightningDelayed(d);
    internal void ValidateOriginalUseLightning(CardUseFrame use) => _resolve().ValidateOriginalUseLightning(use);
}
