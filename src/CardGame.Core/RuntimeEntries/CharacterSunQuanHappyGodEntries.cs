#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunQuanHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanHappyGodRuntime> _resolve;
    private CharacterSunQuanHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunQuanHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanHappyGodRuntime> resolve) => new(resolve);
}
