#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTaishiCiHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterTaishiCiHappyGodRuntime> _resolve;
    private CharacterTaishiCiHappyGodEntries(Func<EngineOwners.CharacterTaishiCiHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTaishiCiHappyGodEntries Create(Func<EngineOwners.CharacterTaishiCiHappyGodRuntime> resolve) => new(resolve);
}
