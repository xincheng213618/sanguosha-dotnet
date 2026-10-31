#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDrawPhaseAttemptsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawPhaseAttemptsRuntime> _resolve;
    private CoreGameEngineDrawPhaseAttemptsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawPhaseAttemptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDrawPhaseAttemptsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDrawPhaseAttemptsRuntime> resolve) => new(resolve);
}
