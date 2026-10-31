#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPangXiMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterPangXiMobileShiRuntime> _resolve;
    private CharacterPangXiMobileShiEntries(Func<EngineOwners.CharacterPangXiMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPangXiMobileShiEntries Create(Func<EngineOwners.CharacterPangXiMobileShiRuntime> resolve) => new(resolve);
}
