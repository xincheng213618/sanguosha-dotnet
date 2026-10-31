#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLouGuiMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterLouGuiMobileShiRuntime> _resolve;
    private CharacterLouGuiMobileShiEntries(Func<EngineOwners.CharacterLouGuiMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLouGuiMobileShiEntries Create(Func<EngineOwners.CharacterLouGuiMobileShiRuntime> resolve) => new(resolve);
}
