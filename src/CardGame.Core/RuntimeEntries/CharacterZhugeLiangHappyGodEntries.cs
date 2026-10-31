#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeLiangHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangHappyGodRuntime> _resolve;
    private CharacterZhugeLiangHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhugeLiangHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangHappyGodRuntime> resolve) => new(resolve);
}
