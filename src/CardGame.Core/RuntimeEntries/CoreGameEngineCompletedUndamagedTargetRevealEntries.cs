#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime))]
internal sealed partial class CoreGameEngineCompletedUndamagedTargetRevealEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime> _resolve;
    private CoreGameEngineCompletedUndamagedTargetRevealEntries(Func<EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCompletedUndamagedTargetRevealEntries Create(Func<EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime> resolve) => new(resolve);
}
