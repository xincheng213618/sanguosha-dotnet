#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawFundedDistinctBasicObserversEntries
{
    internal bool DrawFundedDistinctBasicFrameRidesOn(ResolutionFrame ride, ResolutionFrame beneath) => _resolve().DrawFundedDistinctBasicFrameRidesOn(ride, beneath);
    internal bool DrawFundedDistinctBasicInitialChild(DrawFundedDistinctBasicFrame frame, ResolutionFrame child) => _resolve().DrawFundedDistinctBasicInitialChild(frame, child);
    internal bool DrawFundedDistinctBasicLegacyAlcoholRide(DrawFundedDistinctBasicFrame paid, int dyingIndex, DyingFrame dying) => _resolve().DrawFundedDistinctBasicLegacyAlcoholRide(paid, dyingIndex, dying);
    internal DrawFundedDistinctBasicFrame? DrawFundedDistinctBasicObserverRoot(long? requestedId = default) => _resolve().DrawFundedDistinctBasicObserverRoot(requestedId);
    internal bool DrawFundedDistinctBasicPaidSuffix(int paidIndex) => _resolve().DrawFundedDistinctBasicPaidSuffix(paidIndex);
    internal bool IsDrawFundedDistinctBasicProgramDying() => _resolve().IsDrawFundedDistinctBasicProgramDying();
}
