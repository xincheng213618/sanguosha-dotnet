#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime))]
internal sealed partial class CoreGameEngineCompletedUndamagedTargetRevealEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime> _resolve;
    private CoreGameEngineCompletedUndamagedTargetRevealEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCompletedUndamagedTargetRevealEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime> resolve) => new(resolve);
}
