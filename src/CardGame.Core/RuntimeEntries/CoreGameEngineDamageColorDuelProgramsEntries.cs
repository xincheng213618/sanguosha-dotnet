#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime))]
internal sealed partial class CoreGameEngineDamageColorDuelProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> _resolve;
    private CoreGameEngineDamageColorDuelProgramsEntries(Func<EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageColorDuelProgramsEntries Create(Func<EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> resolve) => new(resolve);
}
