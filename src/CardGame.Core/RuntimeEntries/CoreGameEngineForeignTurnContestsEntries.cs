#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineForeignTurnContestsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignTurnContestsRuntime> _resolve;
    private CoreGameEngineForeignTurnContestsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignTurnContestsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineForeignTurnContestsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForeignTurnContestsRuntime> resolve) => new(resolve);
}
