#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramDamageReductionRuntime))]
internal sealed partial class CoreGameEngineProgramDamageReductionEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramDamageReductionRuntime> _resolve;
    private CoreGameEngineProgramDamageReductionEntries(Func<EngineOwners.CoreGameEngineProgramDamageReductionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramDamageReductionEntries Create(Func<EngineOwners.CoreGameEngineProgramDamageReductionRuntime> resolve) => new(resolve);
}
