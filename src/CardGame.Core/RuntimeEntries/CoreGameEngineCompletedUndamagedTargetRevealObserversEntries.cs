#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCompletedUndamagedTargetRevealObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealObserversRuntime> _resolve;
    private CoreGameEngineCompletedUndamagedTargetRevealObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCompletedUndamagedTargetRevealObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealObserversRuntime> resolve) => new(resolve);
}
