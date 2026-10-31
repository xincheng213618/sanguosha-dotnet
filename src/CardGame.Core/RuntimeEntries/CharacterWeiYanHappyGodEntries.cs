#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWeiYanHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> _resolve;
    private CharacterWeiYanHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWeiYanHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiYanHappyGodRuntime> resolve) => new(resolve);
}
