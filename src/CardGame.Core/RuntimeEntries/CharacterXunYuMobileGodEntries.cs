#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXunYuMobileGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime> _resolve;
    private CharacterXunYuMobileGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXunYuMobileGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXunYuMobileGodRuntime> resolve) => new(resolve);
}
