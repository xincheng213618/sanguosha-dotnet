#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime))]
internal sealed partial class CoreGameEngineProgramDodgeResponsesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> _resolve;
    private CoreGameEngineProgramDodgeResponsesEntries(Func<EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramDodgeResponsesEntries Create(Func<EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> resolve) => new(resolve);
}
