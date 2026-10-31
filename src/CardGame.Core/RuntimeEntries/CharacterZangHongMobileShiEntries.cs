#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZangHongMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterZangHongMobileShiRuntime> _resolve;
    private CharacterZangHongMobileShiEntries(Func<EngineOwners.CharacterZangHongMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZangHongMobileShiEntries Create(Func<EngineOwners.CharacterZangHongMobileShiRuntime> resolve) => new(resolve);
}
