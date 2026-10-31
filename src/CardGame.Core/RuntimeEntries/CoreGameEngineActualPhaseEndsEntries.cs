#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualPhaseEndsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPhaseEndsRuntime> _resolve;
    private CoreGameEngineActualPhaseEndsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPhaseEndsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualPhaseEndsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActualPhaseEndsRuntime> resolve) => new(resolve);
}
