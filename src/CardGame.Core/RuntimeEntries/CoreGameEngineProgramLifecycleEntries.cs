#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramLifecycleRuntime))]
internal sealed partial class CoreGameEngineProgramLifecycleEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramLifecycleRuntime> _resolve;
    private CoreGameEngineProgramLifecycleEntries(Func<EngineOwners.CoreGameEngineProgramLifecycleRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramLifecycleEntries Create(Func<EngineOwners.CoreGameEngineProgramLifecycleRuntime> resolve) => new(resolve);
}
