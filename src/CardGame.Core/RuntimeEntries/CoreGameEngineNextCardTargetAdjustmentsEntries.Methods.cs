#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNextCardTargetAdjustmentsEntries
{
    internal void AddNextCardTargetAdjustmentActions(List<LegalAction> actions, CharacterState actor) => _resolve().AddNextCardTargetAdjustmentActions(actions, actor);
    internal bool CanBeExtraNextCardTarget(CharacterState actor, CharacterState target, LegalAction action, CardKind kind, Suit? effectiveUseSuit = default) => _resolve().CanBeExtraNextCardTarget(actor, target, action, kind, effectiveUseSuit);
    internal void GrantNextCardTargetAdjustment(ProgramSkillFrame frame) => _resolve().GrantNextCardTargetAdjustment(frame);
    internal bool HasLegacyNextCardTargetAdjustment(CharacterState owner) => _resolve().HasLegacyNextCardTargetAdjustment(owner);
    internal bool HasRemainingAdjustedBorrowedSwordTargets(long frameId) => _resolve().HasRemainingAdjustedBorrowedSwordTargets(frameId);
    internal bool HasRemainingAdjustedSimpleTargets(long frameId) => _resolve().HasRemainingAdjustedSimpleTargets(frameId);
    internal void ObserveNextCardTargetAdjustmentEvent(IGameEvent payload) => _resolve().ObserveNextCardTargetAdjustmentEvent(payload);
    internal bool TryContinueAdjustedBorrowedSwordUse(long frameId) => _resolve().TryContinueAdjustedBorrowedSwordUse(frameId);
    internal bool TryContinueAdjustedSimpleCardUse(long frameId) => _resolve().TryContinueAdjustedSimpleCardUse(frameId);
}
