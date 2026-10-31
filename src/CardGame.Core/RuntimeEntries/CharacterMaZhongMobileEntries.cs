#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMaZhongMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaZhongMobileRuntime> _resolve;
    private CharacterMaZhongMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaZhongMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaZhongMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMaZhongMobileRuntime> resolve) => new(resolve);
}
