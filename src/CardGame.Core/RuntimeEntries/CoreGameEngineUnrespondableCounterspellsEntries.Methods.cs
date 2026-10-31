#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUnrespondableCounterspellsEntries
{
    internal void ContinuePolicyCounterspellPayment(long windowId) => _resolve().ContinuePolicyCounterspellPayment(windowId);
    internal CardConversionSource? FreezeUnrespondableCounterspellSource(CharacterState owner, IReadOnlyList<int> physicalCardIds) => _resolve().FreezeUnrespondableCounterspellSource(owner, physicalCardIds);
    internal bool IsPaidCounterspellProgramDying() => _resolve().IsPaidCounterspellProgramDying();
    internal void IssueUnrespondableCounterspell(NullificationWindowFrame window, CardActionContext action, CardConversionSource? frozenSource) => _resolve().IssueUnrespondableCounterspell(window, action, frozenSource);
    internal bool PolicyCounterspellDyingFaceEdge(int index) => _resolve().PolicyCounterspellDyingFaceEdge(index);
    internal bool PolicyCounterspellDyingProgramRidesOn(ProgramSkillFrame response, DyingFrame dying) => _resolve().PolicyCounterspellDyingProgramRidesOn(response, dying);
    internal bool PolicyCounterspellVirtualAlcoholRide(int dyingIndex, DyingFrame dying) => _resolve().PolicyCounterspellVirtualAlcoholRide(dyingIndex, dying);
    internal bool TryBeginPolicyCounterspellPayment(NullificationWindowFrame window, CardActionContext action, CardConversionSource? frozenSource, IReadOnlyList<int> frozenMaterialIds) => _resolve().TryBeginPolicyCounterspellPayment(window, action, frozenSource, frozenMaterialIds);
    internal bool TryFinishUnrespondableCounterspell(NullificationWindowFrame window) => _resolve().TryFinishUnrespondableCounterspell(window);
    internal bool TryGetPolicyCounterspellPaymentRide(NullificationWindowFrame window, scoped out int[] rescueCosts) => _resolve().TryGetPolicyCounterspellPaymentRide(window, out rescueCosts);
}
