#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterFuHuangHouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuHuangHouMobileRuntime> _resolve;
    private CharacterFuHuangHouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuHuangHouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterFuHuangHouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuHuangHouMobileRuntime> resolve) => new(resolve);
}
