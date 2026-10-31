#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNativeFactionChangesRuntime))]
internal sealed partial class CoreGameEngineNativeFactionChangesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNativeFactionChangesRuntime> _resolve;
    private CoreGameEngineNativeFactionChangesEntries(Func<EngineOwners.CoreGameEngineNativeFactionChangesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeFactionChangesEntries Create(Func<EngineOwners.CoreGameEngineNativeFactionChangesRuntime> resolve) => new(resolve);
}
