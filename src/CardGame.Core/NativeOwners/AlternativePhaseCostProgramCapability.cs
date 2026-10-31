#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AlternativePhaseCostProgramCapability(Func<RuntimeEntries.CoreAlternativePhaseCostProgramsEntries> getCoreAlternativePhaseCostProgramsRuntime) : IAlternativePhaseCostProgramHost
{
    public SkillProgramStepOutcome PayOwnedCardOrMarker(ProgramSkillFrame frame, IReadOnlyList<CardZoneKind> zones, PlayerMarkerKind marker) => getCoreAlternativePhaseCostProgramsRuntime().BeginAlternativePhaseCost(frame, zones, marker);
}
