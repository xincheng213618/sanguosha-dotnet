#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDrawPhaseAttemptsRuntime))]
internal sealed partial class CoreGameEngineDrawPhaseAttemptsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawPhaseAttemptsRuntime> _resolve;
    private CoreGameEngineDrawPhaseAttemptsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawPhaseAttemptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDrawPhaseAttemptsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawPhaseAttemptsRuntime> resolve) => new(resolve);
}
