#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGaoXiangMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGaoXiangMobileShiRuntime> _resolve;
    private CharacterGaoXiangMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGaoXiangMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGaoXiangMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGaoXiangMobileShiRuntime> resolve) => new(resolve);
}
