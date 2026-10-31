#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTargetCardPreselectionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetCardPreselectionRuntime> _resolve;
    private CoreGameEngineTargetCardPreselectionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetCardPreselectionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTargetCardPreselectionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetCardPreselectionRuntime> resolve) => new(resolve);
}
