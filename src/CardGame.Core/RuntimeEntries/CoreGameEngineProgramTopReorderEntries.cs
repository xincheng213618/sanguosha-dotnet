#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramTopReorderRuntime))]
internal sealed partial class CoreGameEngineProgramTopReorderEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTopReorderRuntime> _resolve;
    private CoreGameEngineProgramTopReorderEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTopReorderRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramTopReorderEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTopReorderRuntime> resolve) => new(resolve);
}
