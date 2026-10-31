#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramMovementSourceCountsRuntime))]
internal sealed partial class CoreGameEngineProgramMovementSourceCountsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramMovementSourceCountsRuntime> _resolve;
    private CoreGameEngineProgramMovementSourceCountsEntries(Func<EngineOwners.CoreGameEngineProgramMovementSourceCountsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramMovementSourceCountsEntries Create(Func<EngineOwners.CoreGameEngineProgramMovementSourceCountsRuntime> resolve) => new(resolve);
}
