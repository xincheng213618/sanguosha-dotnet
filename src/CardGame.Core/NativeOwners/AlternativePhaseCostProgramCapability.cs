#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AlternativePhaseCostProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreAlternativePhaseCostProgramsRuntime>> getCoreAlternativePhaseCostProgramsRuntime) : IAlternativePhaseCostProgramHost
{
    public SkillProgramStepOutcome PayOwnedCardOrMarker(ProgramSkillFrame frame, IReadOnlyList<CardZoneKind> zones, PlayerMarkerKind marker) => getCoreAlternativePhaseCostProgramsRuntime().Value.BeginAlternativePhaseCost(frame, zones, marker);
}
