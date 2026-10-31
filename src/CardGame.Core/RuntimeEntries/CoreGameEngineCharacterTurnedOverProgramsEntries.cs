#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCharacterTurnedOverProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterTurnedOverProgramsRuntime> _resolve;
    private CoreGameEngineCharacterTurnedOverProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterTurnedOverProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCharacterTurnedOverProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCharacterTurnedOverProgramsRuntime> resolve) => new(resolve);
}
