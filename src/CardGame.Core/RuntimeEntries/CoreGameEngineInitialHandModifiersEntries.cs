#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineInitialHandModifiersRuntime))]
internal sealed partial class CoreGameEngineInitialHandModifiersEntries
{
    private readonly Func<EngineOwners.CoreGameEngineInitialHandModifiersRuntime> _resolve;
    private CoreGameEngineInitialHandModifiersEntries(Func<EngineOwners.CoreGameEngineInitialHandModifiersRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineInitialHandModifiersEntries Create(Func<EngineOwners.CoreGameEngineInitialHandModifiersRuntime> resolve) => new(resolve);
}
