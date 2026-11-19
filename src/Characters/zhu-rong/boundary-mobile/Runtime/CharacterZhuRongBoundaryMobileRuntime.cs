#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CharacterZhuRongBoundaryMobileRuntime(
    ICardZoneQueries zones,
    IProgramMovementWindows movementWindows,
    IProgramCardSetBindings cardSets,
    IProgramRuntimeFrames frames,
    INativeLogicalDiscardMovement movement,
    IMatchParticipantStateAccess participants) : IMobileZhuRongProgramHost
{
}
