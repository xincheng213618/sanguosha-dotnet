#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifeRestorationRuntime))]
internal sealed partial class CoreGameEngineProgramLifeRestorationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifeRestorationRuntime> _resolve;
    private CoreGameEngineProgramLifeRestorationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifeRestorationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramLifeRestorationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifeRestorationRuntime> resolve) => new(resolve);
}
