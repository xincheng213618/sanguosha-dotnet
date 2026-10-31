#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoJiaHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaHappyGodRuntime> _resolve;
    private CharacterGuoJiaHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoJiaHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoJiaHappyGodRuntime> resolve) => new(resolve);
}
