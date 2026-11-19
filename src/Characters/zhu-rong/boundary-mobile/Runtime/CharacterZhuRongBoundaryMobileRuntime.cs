#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CharacterZhuRongBoundaryMobileRuntime(
    ICardZoneQueries zones,
    ICardZoneEntryQueries zoneEntries,
    IProgramMovementWindows movementWindows,
    IProgramRuntimeFrames frames,
    INativeLogicalDiscardMovement movement,
    IMatchParticipantStateAccess participants) : IMobileZhuRongProgramHost
{
}
