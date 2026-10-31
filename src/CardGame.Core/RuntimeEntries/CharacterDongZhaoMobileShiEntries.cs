#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDongZhaoMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongZhaoMobileShiRuntime> _resolve;
    private CharacterDongZhaoMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongZhaoMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDongZhaoMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongZhaoMobileShiRuntime> resolve) => new(resolve);
}
