#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeLogicalDiscardOrigins(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDiscardOriginsEntries> getCoreGameEngineProgramDiscardOriginsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries> getCoreGameEngineEphemeralCardMaterialsRuntime) : INativeLogicalDiscardOrigins
{
    public bool IsOwnedDiscardSource(CardLocation location) => global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime.IsOwnedDiscardSource(location);
    public bool IsProgramDiscardOriginReason(CardMoveReason reason) => global::CardGame.Core.EngineOwners.CoreGameEngineProgramDiscardOriginsRuntime.IsProgramDiscardOriginReason(reason);
    public CardLocation? GetProgramDiscardSource(CardMovementRecord movement) => getCoreGameEngineProgramDiscardOriginsRuntime().GetProgramDiscardSource(movement);
    public bool IsEphemeralCardDestroyedMaterial(CardMovementRecord movement) => getCoreGameEngineEphemeralCardMaterialsRuntime().IsEphemeralCardDestroyedMaterial(movement);
}
