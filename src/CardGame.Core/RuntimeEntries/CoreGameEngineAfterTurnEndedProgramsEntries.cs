#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime))]
internal sealed partial class CoreGameEngineAfterTurnEndedProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime> _resolve;
    private CoreGameEngineAfterTurnEndedProgramsEntries(Func<EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAfterTurnEndedProgramsEntries Create(Func<EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime> resolve) => new(resolve);
}
