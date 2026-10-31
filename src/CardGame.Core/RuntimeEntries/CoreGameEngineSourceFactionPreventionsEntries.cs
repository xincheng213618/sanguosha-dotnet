#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceFactionPreventionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceFactionPreventionsRuntime> _resolve;
    private CoreGameEngineSourceFactionPreventionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceFactionPreventionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceFactionPreventionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceFactionPreventionsRuntime> resolve) => new(resolve);
}
