#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeFactionChangesRuntime))]
internal sealed partial class CoreGameEngineNativeFactionChangesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeFactionChangesRuntime> _resolve;
    private CoreGameEngineNativeFactionChangesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeFactionChangesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeFactionChangesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeFactionChangesRuntime> resolve) => new(resolve);
}
