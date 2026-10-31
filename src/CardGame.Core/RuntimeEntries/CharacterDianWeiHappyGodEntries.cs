#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDianWeiHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDianWeiHappyGodRuntime> _resolve;
    private CharacterDianWeiHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDianWeiHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDianWeiHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDianWeiHappyGodRuntime> resolve) => new(resolve);
}
