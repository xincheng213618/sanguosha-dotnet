#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCharacterTurnedOverProgramsRuntime))]
internal sealed partial class CoreGameEngineCharacterTurnedOverProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCharacterTurnedOverProgramsRuntime> _resolve;
    private CoreGameEngineCharacterTurnedOverProgramsEntries(Func<EngineOwners.CoreGameEngineCharacterTurnedOverProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCharacterTurnedOverProgramsEntries Create(Func<EngineOwners.CoreGameEngineCharacterTurnedOverProgramsRuntime> resolve) => new(resolve);
}
