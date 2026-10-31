#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreActualTurnsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> _resolve;
    private CoreActualTurnsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreActualTurnsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> resolve) => new(resolve);
}
