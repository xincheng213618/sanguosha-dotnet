#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuSuHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterLuSuHappyGodRuntime> _resolve;
    private CharacterLuSuHappyGodEntries(Func<EngineOwners.CharacterLuSuHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuSuHappyGodEntries Create(Func<EngineOwners.CharacterLuSuHappyGodRuntime> resolve) => new(resolve);
}
