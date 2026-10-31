#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineExtendedViewAsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime> _resolve;
    private CoreGameEngineExtendedViewAsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineExtendedViewAsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtendedViewAsRuntime> resolve) => new(resolve);
}
