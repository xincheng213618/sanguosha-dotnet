#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime))]
internal sealed partial class CoreGameEngineDamageColorDuelProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> _resolve;
    private CoreGameEngineDamageColorDuelProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageColorDuelProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> resolve) => new(resolve);
}
