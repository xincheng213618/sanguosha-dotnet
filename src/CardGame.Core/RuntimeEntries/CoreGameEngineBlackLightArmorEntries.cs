#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBlackLightArmorEntries
{
    private readonly Func<EngineOwners.CoreGameEngineBlackLightArmorRuntime> _resolve;
    private CoreGameEngineBlackLightArmorEntries(Func<EngineOwners.CoreGameEngineBlackLightArmorRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBlackLightArmorEntries Create(Func<EngineOwners.CoreGameEngineBlackLightArmorRuntime> resolve) => new(resolve);
}
