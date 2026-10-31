#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeLiangHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterZhugeLiangHappyGodRuntime> _resolve;
    private CharacterZhugeLiangHappyGodEntries(Func<EngineOwners.CharacterZhugeLiangHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhugeLiangHappyGodEntries Create(Func<EngineOwners.CharacterZhugeLiangHappyGodRuntime> resolve) => new(resolve);
}
