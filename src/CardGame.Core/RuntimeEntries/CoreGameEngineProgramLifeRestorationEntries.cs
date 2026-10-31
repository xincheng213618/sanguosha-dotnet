#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramLifeRestorationRuntime))]
internal sealed partial class CoreGameEngineProgramLifeRestorationEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramLifeRestorationRuntime> _resolve;
    private CoreGameEngineProgramLifeRestorationEntries(Func<EngineOwners.CoreGameEngineProgramLifeRestorationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramLifeRestorationEntries Create(Func<EngineOwners.CoreGameEngineProgramLifeRestorationRuntime> resolve) => new(resolve);
}
