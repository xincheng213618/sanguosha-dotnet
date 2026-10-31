#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoShuangMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoShuangMobileShiRuntime> _resolve;
    private CharacterCaoShuangMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoShuangMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoShuangMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoShuangMobileShiRuntime> resolve) => new(resolve);
}
