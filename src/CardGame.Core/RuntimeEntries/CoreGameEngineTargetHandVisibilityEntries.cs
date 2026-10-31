#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTargetHandVisibilityEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetHandVisibilityRuntime> _resolve;
    private CoreGameEngineTargetHandVisibilityEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetHandVisibilityRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTargetHandVisibilityEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTargetHandVisibilityRuntime> resolve) => new(resolve);
}
