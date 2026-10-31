#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHanDangMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanDangMouMobileRuntime> _resolve;
    private CharacterHanDangMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanDangMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHanDangMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanDangMouMobileRuntime> resolve) => new(resolve);
}
