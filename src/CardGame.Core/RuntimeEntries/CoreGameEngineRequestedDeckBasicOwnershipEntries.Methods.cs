#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRequestedDeckBasicOwnershipEntries
{
    internal void AssertRequestedDeckBasicFrame(RequestedDeckBasicFrame view) => _resolve().AssertRequestedDeckBasicFrame(view);
    internal bool HasValidRequestedDeckBasicMaterial(ResolutionFrame owner) => _resolve().HasValidRequestedDeckBasicMaterial(owner);
    internal bool IsRequestedDeckBasicRescueCost(CardUseFrame use, DyingFrame dying, CardActionCost cost) => _resolve().IsRequestedDeckBasicRescueCost(use, dying, cost);
    internal CardActionType RequestedDeckBasicActionType(RequestedDeckBasicIntent intent) => _resolve().RequestedDeckBasicActionType(intent);
    internal bool RequestedDeckBasicFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _resolve().RequestedDeckBasicFrameRidesOn(child, parent);
}
