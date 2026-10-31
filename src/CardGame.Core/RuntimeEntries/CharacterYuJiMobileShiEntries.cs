#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuJiMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterYuJiMobileShiRuntime> _resolve;
    private CharacterYuJiMobileShiEntries(Func<EngineOwners.CharacterYuJiMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYuJiMobileShiEntries Create(Func<EngineOwners.CharacterYuJiMobileShiRuntime> resolve) => new(resolve);
}
