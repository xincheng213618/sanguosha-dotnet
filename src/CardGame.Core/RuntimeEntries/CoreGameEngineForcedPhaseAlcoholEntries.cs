#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineForcedPhaseAlcoholRuntime))]
internal sealed partial class CoreGameEngineForcedPhaseAlcoholEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForcedPhaseAlcoholRuntime> _resolve;
    private CoreGameEngineForcedPhaseAlcoholEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForcedPhaseAlcoholRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineForcedPhaseAlcoholEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineForcedPhaseAlcoholRuntime> resolve) => new(resolve);
}
