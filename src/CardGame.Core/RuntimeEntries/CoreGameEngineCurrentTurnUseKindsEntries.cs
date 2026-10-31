#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime))]
internal sealed partial class CoreGameEngineCurrentTurnUseKindsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime> _resolve;
    private CoreGameEngineCurrentTurnUseKindsEntries(Func<EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentTurnUseKindsEntries Create(Func<EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime> resolve) => new(resolve);
}
