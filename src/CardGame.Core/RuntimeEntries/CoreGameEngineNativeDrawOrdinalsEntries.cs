#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawOrdinalsRuntime))]
internal sealed partial class CoreGameEngineNativeDrawOrdinalsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawOrdinalsRuntime> _resolve;
    private CoreGameEngineNativeDrawOrdinalsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawOrdinalsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNativeDrawOrdinalsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawOrdinalsRuntime> resolve) => new(resolve);
}
