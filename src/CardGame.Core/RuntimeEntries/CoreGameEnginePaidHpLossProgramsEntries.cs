#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePaidHpLossProgramsRuntime))]
internal sealed partial class CoreGameEnginePaidHpLossProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidHpLossProgramsRuntime> _resolve;
    private CoreGameEnginePaidHpLossProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidHpLossProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePaidHpLossProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidHpLossProgramsRuntime> resolve) => new(resolve);
}
