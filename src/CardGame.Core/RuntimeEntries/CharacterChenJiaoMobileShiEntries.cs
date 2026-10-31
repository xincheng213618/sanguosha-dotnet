#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenJiaoMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterChenJiaoMobileShiRuntime> _resolve;
    private CharacterChenJiaoMobileShiEntries(Func<EngineOwners.CharacterChenJiaoMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChenJiaoMobileShiEntries Create(Func<EngineOwners.CharacterChenJiaoMobileShiRuntime> resolve) => new(resolve);
}
