#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouYuMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterZhouYuMobileShiRuntime> _resolve;
    private CharacterZhouYuMobileShiEntries(Func<EngineOwners.CharacterZhouYuMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouYuMobileShiEntries Create(Func<EngineOwners.CharacterZhouYuMobileShiRuntime> resolve) => new(resolve);
}
