#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineThirdPartyHandGainsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineThirdPartyHandGainsRuntime> _resolve;
    private CoreGameEngineThirdPartyHandGainsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineThirdPartyHandGainsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineThirdPartyHandGainsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineThirdPartyHandGainsRuntime> resolve) => new(resolve);
}
