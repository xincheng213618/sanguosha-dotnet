#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramLifecycleEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> _resolve;
    private CoreGameEngineProgramLifecycleEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramLifecycleEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime> resolve) => new(resolve);
}
