#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicObserversEntries
{
    internal void AssertDrawFundedDistinctBasicFramesAndUses() => _resolve().AssertDrawFundedDistinctBasicFramesAndUses();
    internal bool DrawFundedDistinctBasicFrameRidesOn(global::CardGame.Core.ResolutionFrame ride, global::CardGame.Core.ResolutionFrame beneath) => _resolve().DrawFundedDistinctBasicFrameRidesOn(ride, beneath);
    internal bool DrawFundedDistinctBasicInitialChild(global::CardGame.Core.DrawFundedDistinctBasicFrame frame, global::CardGame.Core.ResolutionFrame child) => _resolve().DrawFundedDistinctBasicInitialChild(frame, child);
    internal global::CardGame.Core.PendingDecision? DrawFundedDistinctBasicInvariantDecision(global::CardGame.Core.PendingDecision? nativeDecision) => _resolve().DrawFundedDistinctBasicInvariantDecision(nativeDecision);
    internal global::CardGame.Core.ResolutionFrame? DrawFundedDistinctBasicInvariantTop(global::CardGame.Core.ResolutionFrame? nativeTop) => _resolve().DrawFundedDistinctBasicInvariantTop(nativeTop);
    internal bool DrawFundedDistinctBasicLegacyAlcoholRide(global::CardGame.Core.DrawFundedDistinctBasicFrame paid, int dyingIndex, global::CardGame.Core.DyingFrame dying) => _resolve().DrawFundedDistinctBasicLegacyAlcoholRide(paid, dyingIndex, dying);
    internal global::CardGame.Core.DrawFundedDistinctBasicFrame? DrawFundedDistinctBasicObserverRoot(long? requestedId = default) => _resolve().DrawFundedDistinctBasicObserverRoot(requestedId);
    internal bool DrawFundedDistinctBasicPaidSuffix(int paidIndex) => _resolve().DrawFundedDistinctBasicPaidSuffix(paidIndex);
    internal bool HasDrawFundedDistinctBasicDamageObserver(long damageId) => _resolve().HasDrawFundedDistinctBasicDamageObserver(damageId);
    internal bool HasDrawFundedDistinctBasicObserver(long cardUseId) => _resolve().HasDrawFundedDistinctBasicObserver(cardUseId);
    internal bool IsDrawFundedDistinctBasicProgramDying() => _resolve().IsDrawFundedDistinctBasicProgramDying();
}
