#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAfterTurnEndedProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime> _resolve;
    private CoreGameEngineAfterTurnEndedProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAfterTurnEndedProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAfterTurnEndedProgramsRuntime> resolve) => new(resolve);
}
