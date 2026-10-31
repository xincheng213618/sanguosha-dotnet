#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime))]
internal sealed partial class CoreActualTurnsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> _resolve;
    private CoreActualTurnsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreActualTurnsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreActualTurnsRuntime> resolve) => new(resolve);
}
