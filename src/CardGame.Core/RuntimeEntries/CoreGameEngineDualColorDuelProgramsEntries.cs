#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDualColorDuelProgramsRuntime))]
internal sealed partial class CoreGameEngineDualColorDuelProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDualColorDuelProgramsRuntime> _resolve;
    private CoreGameEngineDualColorDuelProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDualColorDuelProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDualColorDuelProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDualColorDuelProgramsRuntime> resolve) => new(resolve);
}
