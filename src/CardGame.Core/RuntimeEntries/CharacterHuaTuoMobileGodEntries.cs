#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuaTuoMobileGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoMobileGodRuntime> _resolve;
    private CharacterHuaTuoMobileGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuaTuoMobileGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoMobileGodRuntime> resolve) => new(resolve);
}
