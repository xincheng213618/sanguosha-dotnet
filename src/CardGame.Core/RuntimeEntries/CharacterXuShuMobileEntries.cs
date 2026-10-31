#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuShuMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuShuMobileRuntime> _resolve;
    private CharacterXuShuMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuShuMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXuShuMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuShuMobileRuntime> resolve) => new(resolve);
}
