#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterQuanCongBoundaryMobileCorePlayerViewProjectionOperations : IProgramRuntimeOperations
{
    GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}
