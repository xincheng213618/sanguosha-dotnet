#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime))]
internal sealed partial class CoreGameEngineEphemeralCardMaterialsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> _resolve;
    private CoreGameEngineEphemeralCardMaterialsEntries(Func<EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineEphemeralCardMaterialsEntries Create(Func<EngineOwners.CoreGameEngineEphemeralCardMaterialsRuntime> resolve) => new(resolve);
}
