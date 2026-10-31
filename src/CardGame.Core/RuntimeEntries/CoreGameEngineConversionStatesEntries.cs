#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineConversionStatesRuntime))]
internal sealed partial class CoreGameEngineConversionStatesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConversionStatesRuntime> _resolve;
    private CoreGameEngineConversionStatesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConversionStatesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineConversionStatesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConversionStatesRuntime> resolve) => new(resolve);
}
