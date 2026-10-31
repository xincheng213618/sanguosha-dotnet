#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineConversionStatesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConversionStatesRuntime> _resolve;
    private CoreGameEngineConversionStatesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConversionStatesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineConversionStatesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineConversionStatesRuntime> resolve) => new(resolve);
}
