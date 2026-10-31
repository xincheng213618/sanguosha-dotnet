#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYangHongMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterYangHongMobileShiRuntime> _resolve;
    private CharacterYangHongMobileShiEntries(Func<EngineOwners.CharacterYangHongMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYangHongMobileShiEntries Create(Func<EngineOwners.CharacterYangHongMobileShiRuntime> resolve) => new(resolve);
}
