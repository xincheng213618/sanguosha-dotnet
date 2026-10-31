#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDualColorDuelProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDualColorDuelProgramsRuntime> _resolve;
    private CoreGameEngineDualColorDuelProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDualColorDuelProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDualColorDuelProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDualColorDuelProgramsRuntime> resolve) => new(resolve);
}
