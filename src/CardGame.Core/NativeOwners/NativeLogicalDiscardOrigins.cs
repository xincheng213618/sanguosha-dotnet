#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeLogicalDiscardOrigins(Func<RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries> getCoreGameEngineProgramDiscardOriginsRuntime, Func<RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries> getCoreGameEngineEphemeralCardMaterialsRuntime) : INativeLogicalDiscardOrigins
{
    public bool IsOwnedDiscardSource(CardLocation location) => EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime.IsOwnedDiscardSource(location);
    public bool IsProgramDiscardOriginReason(CardMoveReason reason) => EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime.IsProgramDiscardOriginReason(reason);
    public CardLocation? GetProgramDiscardSource(CardMovementRecord movement) => getCoreGameEngineProgramDiscardOriginsRuntime().GetProgramDiscardSource(movement);
    public bool IsEphemeralCardDestroyedMaterial(CardMovementRecord movement) => getCoreGameEngineEphemeralCardMaterialsRuntime().IsEphemeralCardDestroyedMaterial(movement);
}
