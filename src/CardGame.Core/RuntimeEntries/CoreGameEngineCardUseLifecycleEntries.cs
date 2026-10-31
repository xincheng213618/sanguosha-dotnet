#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime))]
internal sealed partial class CoreGameEngineCardUseLifecycleEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> _resolve;
    private CoreGameEngineCardUseLifecycleEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardUseLifecycleEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardUseLifecycleRuntime> resolve) => new(resolve);
}
