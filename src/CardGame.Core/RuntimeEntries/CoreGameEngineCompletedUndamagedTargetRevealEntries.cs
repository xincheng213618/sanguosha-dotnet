#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCompletedUndamagedTargetRevealEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime> _resolve;
    private CoreGameEngineCompletedUndamagedTargetRevealEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCompletedUndamagedTargetRevealEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime> resolve) => new(resolve);
}
