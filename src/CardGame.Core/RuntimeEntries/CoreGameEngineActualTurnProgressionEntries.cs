#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnProgressionRuntime))]
internal sealed partial class CoreGameEngineActualTurnProgressionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnProgressionRuntime> _resolve;
    private CoreGameEngineActualTurnProgressionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnProgressionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualTurnProgressionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualTurnProgressionRuntime> resolve) => new(resolve);
}
