#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRuntimeRuntime))]
internal sealed partial class CoreGameEngineRuntimeEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRuntimeRuntime> _resolve;
    private CoreGameEngineRuntimeEntries(Func<EngineOwners.CoreGameEngineRuntimeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRuntimeEntries Create(Func<EngineOwners.CoreGameEngineRuntimeRuntime> resolve) => new(resolve);
}
