#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFeibaiEffectiveUseKindsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFeibaiEffectiveUseKindsRuntime> _resolve;
    private CoreGameEngineFeibaiEffectiveUseKindsEntries(Func<EngineOwners.CoreGameEngineFeibaiEffectiveUseKindsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFeibaiEffectiveUseKindsEntries Create(Func<EngineOwners.CoreGameEngineFeibaiEffectiveUseKindsRuntime> resolve) => new(resolve);
}
