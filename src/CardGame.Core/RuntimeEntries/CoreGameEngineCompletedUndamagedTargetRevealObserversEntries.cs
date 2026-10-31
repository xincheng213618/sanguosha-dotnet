#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealObserversRuntime))]
internal sealed partial class CoreGameEngineCompletedUndamagedTargetRevealObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealObserversRuntime> _resolve;
    private CoreGameEngineCompletedUndamagedTargetRevealObserversEntries(Func<EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCompletedUndamagedTargetRevealObserversEntries Create(Func<EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealObserversRuntime> resolve) => new(resolve);
}
