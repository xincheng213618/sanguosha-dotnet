#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuHuangMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuHuangMouMobileRuntime> _resolve;
    private CharacterXuHuangMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuHuangMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXuHuangMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuHuangMouMobileRuntime> resolve) => new(resolve);
}
