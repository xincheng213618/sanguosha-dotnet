#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNextCardTargetAdjustmentsEntries
{
    internal void AddNextCardTargetAdjustmentActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor) => _resolve().AddNextCardTargetAdjustmentActions(actions, actor);
    internal bool CanBeExtraNextCardTarget(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.LegalAction action, global::CardGame.Core.CardKind kind, global::CardGame.Core.Suit? effectiveUseSuit = default) => _resolve().CanBeExtraNextCardTarget(actor, target, action, kind, effectiveUseSuit);
    internal void GrantNextCardTargetAdjustment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().GrantNextCardTargetAdjustment(frame);
    internal bool HasLegacyNextCardTargetAdjustment(global::CardGame.Core.CharacterState owner) => _resolve().HasLegacyNextCardTargetAdjustment(owner);
    internal bool HasRemainingAdjustedBorrowedSwordTargets(long frameId) => _resolve().HasRemainingAdjustedBorrowedSwordTargets(frameId);
    internal bool HasRemainingAdjustedSimpleTargets(long frameId) => _resolve().HasRemainingAdjustedSimpleTargets(frameId);
    internal void ObserveNextCardTargetAdjustmentEvent(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveNextCardTargetAdjustmentEvent(payload);
    internal bool TryContinueAdjustedBorrowedSwordUse(long frameId) => _resolve().TryContinueAdjustedBorrowedSwordUse(frameId);
    internal bool TryContinueAdjustedSimpleCardUse(long frameId) => _resolve().TryContinueAdjustedSimpleCardUse(frameId);
}
