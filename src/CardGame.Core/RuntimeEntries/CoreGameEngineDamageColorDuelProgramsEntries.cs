#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageColorDuelProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> _resolve;
    private CoreGameEngineDamageColorDuelProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageColorDuelProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageColorDuelProgramsRuntime> resolve) => new(resolve);
}
