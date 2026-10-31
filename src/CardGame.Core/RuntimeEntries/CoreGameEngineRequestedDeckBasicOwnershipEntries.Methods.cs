#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRequestedDeckBasicOwnershipEntries
{
    internal void AssertRequestedDeckBasicFrame(global::CardGame.Core.RequestedDeckBasicFrame view) => _resolve().AssertRequestedDeckBasicFrame(view);
    internal bool HasValidRequestedDeckBasicMaterial(global::CardGame.Core.ResolutionFrame owner) => _resolve().HasValidRequestedDeckBasicMaterial(owner);
    internal bool IsRequestedDeckBasicRescueCost(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DyingFrame dying, global::CardGame.Core.CardActionCost cost) => _resolve().IsRequestedDeckBasicRescueCost(use, dying, cost);
    internal global::CardGame.Core.CardActionType RequestedDeckBasicActionType(global::CardGame.Core.RequestedDeckBasicIntent intent) => _resolve().RequestedDeckBasicActionType(intent);
    internal bool RequestedDeckBasicFrameRidesOn(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => _resolve().RequestedDeckBasicFrameRidesOn(child, parent);
}
