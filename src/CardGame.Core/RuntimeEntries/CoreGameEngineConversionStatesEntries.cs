#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineConversionStatesRuntime))]
internal sealed partial class CoreGameEngineConversionStatesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineConversionStatesRuntime> _resolve;
    private CoreGameEngineConversionStatesEntries(Func<EngineOwners.CoreGameEngineConversionStatesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineConversionStatesEntries Create(Func<EngineOwners.CoreGameEngineConversionStatesRuntime> resolve) => new(resolve);
}
