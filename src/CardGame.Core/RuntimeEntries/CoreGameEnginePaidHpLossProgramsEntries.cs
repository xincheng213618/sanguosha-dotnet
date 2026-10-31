#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePaidHpLossProgramsRuntime))]
internal sealed partial class CoreGameEnginePaidHpLossProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePaidHpLossProgramsRuntime> _resolve;
    private CoreGameEnginePaidHpLossProgramsEntries(Func<EngineOwners.CoreGameEnginePaidHpLossProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePaidHpLossProgramsEntries Create(Func<EngineOwners.CoreGameEnginePaidHpLossProgramsRuntime> resolve) => new(resolve);
}
