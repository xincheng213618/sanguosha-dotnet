#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineThirdPartyHandGainsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineThirdPartyHandGainsRuntime> _resolve;
    private CoreGameEngineThirdPartyHandGainsEntries(Func<EngineOwners.CoreGameEngineThirdPartyHandGainsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineThirdPartyHandGainsEntries Create(Func<EngineOwners.CoreGameEngineThirdPartyHandGainsRuntime> resolve) => new(resolve);
}
