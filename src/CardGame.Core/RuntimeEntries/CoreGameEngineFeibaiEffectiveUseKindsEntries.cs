#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFeibaiEffectiveUseKindsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFeibaiEffectiveUseKindsRuntime> _resolve;
    private CoreGameEngineFeibaiEffectiveUseKindsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFeibaiEffectiveUseKindsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFeibaiEffectiveUseKindsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFeibaiEffectiveUseKindsRuntime> resolve) => new(resolve);
}
