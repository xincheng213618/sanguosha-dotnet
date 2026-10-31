#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeUsedDiscardOriginsRuntime))]
internal sealed partial class CoreGameEngineNativeUsedDiscardOriginsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeUsedDiscardOriginsRuntime> _resolve;
    private CoreGameEngineNativeUsedDiscardOriginsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeUsedDiscardOriginsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeUsedDiscardOriginsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeUsedDiscardOriginsRuntime> resolve) => new(resolve);
}
