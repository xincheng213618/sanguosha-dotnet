#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDrawPhaseAttemptsRuntime))]
internal sealed partial class CoreGameEngineDrawPhaseAttemptsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDrawPhaseAttemptsRuntime> _resolve;
    private CoreGameEngineDrawPhaseAttemptsEntries(Func<EngineOwners.CoreGameEngineDrawPhaseAttemptsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDrawPhaseAttemptsEntries Create(Func<EngineOwners.CoreGameEngineDrawPhaseAttemptsRuntime> resolve) => new(resolve);
}
