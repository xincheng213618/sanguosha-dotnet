#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUnrespondableCounterspellsEntries
{
    internal void ContinuePolicyCounterspellPayment(long windowId) => _resolve().ContinuePolicyCounterspellPayment(windowId);
    internal global::CardGame.Core.CardConversionSource? FreezeUnrespondableCounterspellSource(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> physicalCardIds) => _resolve().FreezeUnrespondableCounterspellSource(owner, physicalCardIds);
    internal bool IsPaidCounterspellProgramDying() => _resolve().IsPaidCounterspellProgramDying();
    internal void IssueUnrespondableCounterspell(global::CardGame.Core.NullificationWindowFrame window, global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardConversionSource? frozenSource) => _resolve().IssueUnrespondableCounterspell(window, action, frozenSource);
    internal bool PolicyCounterspellDyingFaceEdge(int index) => _resolve().PolicyCounterspellDyingFaceEdge(index);
    internal bool PolicyCounterspellDyingProgramRidesOn(global::CardGame.Core.ProgramSkillFrame response, global::CardGame.Core.DyingFrame dying) => _resolve().PolicyCounterspellDyingProgramRidesOn(response, dying);
    internal bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, global::CardGame.Core.DyingFrame dying) => _resolve().PolicyCounterspellVirtualAlcoholRide(dyingIndex, dying);
    internal bool TryBeginPolicyCounterspellPayment(global::CardGame.Core.NullificationWindowFrame window, global::CardGame.Core.CardActionContext action, global::CardGame.Core.CardConversionSource? frozenSource, global::System.Collections.Generic.IReadOnlyList<int> frozenMaterialIds) => _resolve().TryBeginPolicyCounterspellPayment(window, action, frozenSource, frozenMaterialIds);
    internal bool TryFinishUnrespondableCounterspell(global::CardGame.Core.NullificationWindowFrame window) => _resolve().TryFinishUnrespondableCounterspell(window);
    internal bool TryGetPolicyCounterspellPaymentRide(global::CardGame.Core.NullificationWindowFrame window, scoped out int[] rescueCosts) => _resolve().TryGetPolicyCounterspellPaymentRide(window, out rescueCosts);
}
