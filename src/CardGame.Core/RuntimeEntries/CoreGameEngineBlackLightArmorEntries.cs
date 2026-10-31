#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBlackLightArmorEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBlackLightArmorRuntime> _resolve;
    private CoreGameEngineBlackLightArmorEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBlackLightArmorRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBlackLightArmorEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBlackLightArmorRuntime> resolve) => new(resolve);
}
