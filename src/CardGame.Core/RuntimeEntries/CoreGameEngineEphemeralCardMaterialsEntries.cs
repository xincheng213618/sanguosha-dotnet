#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime))]
internal sealed partial class CoreGameEngineEphemeralCardMaterialsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> _resolve;
    private CoreGameEngineEphemeralCardMaterialsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEphemeralCardMaterialsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> resolve) => new(resolve);
}
