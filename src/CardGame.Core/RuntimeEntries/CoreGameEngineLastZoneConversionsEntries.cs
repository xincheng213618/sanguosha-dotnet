#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLastZoneConversionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLastZoneConversionsRuntime> _resolve;
    private CoreGameEngineLastZoneConversionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLastZoneConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLastZoneConversionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLastZoneConversionsRuntime> resolve) => new(resolve);
}
