#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOriginalUseLightningEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PlayerMarkerSnapshot>? AppendOriginalUseLightningMarker(int seat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PlayerMarkerSnapshot>? markers) => _resolve().AppendOriginalUseLightningMarker(seat, markers);
    internal global::CardGame.Core.OriginalUseLightningAttachment AttachOriginalUseLightning(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardConversionSource source, string gameplayHash, int target) => _resolve().AttachOriginalUseLightning(use, source, gameplayHash, target);
    internal void BeginOriginalUseLightning(long useId, global::CardGame.Core.CardConversionSource source, string gameplayHash) => _resolve().BeginOriginalUseLightning(useId, source, gameplayHash);
    internal bool BeginOriginalUseLightningFinalized(long id, global::CardGame.Core.Card card, global::CardGame.Core.LegalActionKind actionKind) => _resolve().BeginOriginalUseLightningFinalized(id, card, actionKind);
    internal void BeginOriginalUseLightningJudgment(global::CardGame.Core.DelayedEffectContinuation delayed) => _resolve().BeginOriginalUseLightningJudgment(delayed);
    internal global::CardGame.Core.ProgramSkillFrame BindNativeDelayedAttackChild(global::CardGame.Core.ProgramSkillFrame child, long judgmentId) => _resolve().BindNativeDelayedAttackChild(child, judgmentId);
    internal void ClearUnavailableOriginalUseLightning() => _resolve().ClearUnavailableOriginalUseLightning();
    internal void CompleteOriginalUseLightningJudgment(global::CardGame.Core.JudgmentFrame pending, bool hit) => _resolve().CompleteOriginalUseLightningJudgment(pending, hit);
    internal void CompleteOriginalUseLightningNullification(global::CardGame.Core.DelayedEffectContinuation delayed, bool nullified, long windowId) => _resolve().CompleteOriginalUseLightningNullification(delayed, nullified, windowId);
    internal void ContinueOriginalUseLightningFinalized(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().ContinueOriginalUseLightningFinalized(window);
    internal bool ExactOriginalUseLightning(global::CardGame.Core.OriginalUseLightningAttachment a) => _resolve().ExactOriginalUseLightning(a);
    internal bool FinishOriginalUseLightningAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().FinishOriginalUseLightningAttack(attack);
    internal bool HasHeldOriginalUseLightning(global::CardGame.Core.CharacterState player) => _resolve().HasHeldOriginalUseLightning(player);
    internal bool HasNativeDelayedAttackChild(long judgmentId) => _resolve().HasNativeDelayedAttackChild(judgmentId);
    internal global::CardGame.Core.OriginalUseLightningAttachment? HeldOriginalUseLightning(long id) => _resolve().HeldOriginalUseLightning(id);
    internal bool IsOriginalUseLightning(long id) => _resolve().IsOriginalUseLightning(id);
    internal bool IsOriginalUseLightningAttackConsistent(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().IsOriginalUseLightningAttackConsistent(attack, processing);
    internal bool IsOriginalUseLightningFinalizedWindow(global::CardGame.Core.ProgramCardTriggerWindowFrame window) => _resolve().IsOriginalUseLightningFinalizedWindow(window);
    internal bool IsOriginalUseLightningJudgment(global::CardGame.Core.JudgmentFrame j) => _resolve().IsOriginalUseLightningJudgment(j);
    internal bool MatchesOriginalUseLightningAction(long id, global::CardGame.Core.CardActionContext action, int cardId) => _resolve().MatchesOriginalUseLightningAction(id, action, cardId);
    internal global::System.Collections.Generic.IReadOnlyList<int> OriginalUseLightningPlacementTargets(global::CardGame.Core.NativeOwners.JizhiResolution pending) => _resolve().OriginalUseLightningPlacementTargets(pending);
    internal bool PlaceOriginalUseLightning(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().PlaceOriginalUseLightning(pending);
    internal void PreserveNativeDelayedAttackCompletion(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().PreserveNativeDelayedAttackCompletion(completion);
    internal void RemoveOriginalUseLightning(global::CardGame.Core.OriginalUseLightningAttachment a, string reason) => _resolve().RemoveOriginalUseLightning(a, reason);
    internal void ReplaceHeldOriginalUseLightning(global::CardGame.Core.OriginalUseLightningAttachment a) => _resolve().ReplaceHeldOriginalUseLightning(a);
    internal void ResumeNativeDelayedAttackCompletion(long judgmentId) => _resolve().ResumeNativeDelayedAttackCompletion(judgmentId);
    internal bool SkipUnavailableOriginalUseLightning(global::CardGame.Core.OriginalUseLightningAttachment a, int? excludedCard) => _resolve().SkipUnavailableOriginalUseLightning(a, excludedCard);
    internal void TransferOriginalUseLightning(global::CardGame.Core.OriginalUseLightningAttachment a, long judgmentId, int? cardId, int? excludedCard) => _resolve().TransferOriginalUseLightning(a, judgmentId, cardId, excludedCard);
    internal bool TryBeginHeldOriginalUseLightning(global::CardGame.Core.CharacterState current, int? excludedCard) => _resolve().TryBeginHeldOriginalUseLightning(current, excludedCard);
    internal bool TryContinueOriginalUseLightningPlacement(long id) => _resolve().TryContinueOriginalUseLightningPlacement(id);
    internal bool ValidOriginalUseLightning(global::CardGame.Core.OriginalUseLightningAttachment a) => _resolve().ValidOriginalUseLightning(a);
    internal bool ValidOriginalUseLightningDelayed(global::CardGame.Core.DelayedEffectContinuation d) => _resolve().ValidOriginalUseLightningDelayed(d);
    internal void ValidateOriginalUseLightning(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateOriginalUseLightning(use);
}
