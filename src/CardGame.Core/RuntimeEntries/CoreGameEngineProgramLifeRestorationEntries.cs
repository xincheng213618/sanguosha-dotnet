#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramLifeRestorationEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifeRestorationRuntime> _resolve;
    private CoreGameEngineProgramLifeRestorationEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifeRestorationRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramLifeRestorationEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifeRestorationRuntime> resolve) => new(resolve);
}
