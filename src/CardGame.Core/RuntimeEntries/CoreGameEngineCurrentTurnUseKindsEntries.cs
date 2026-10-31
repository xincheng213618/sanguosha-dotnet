#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCurrentTurnUseKindsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime> _resolve;
    private CoreGameEngineCurrentTurnUseKindsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentTurnUseKindsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCurrentTurnUseKindsRuntime> resolve) => new(resolve);
}
