#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGanNingHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterGanNingHappyGodRuntime> _resolve;
    private CharacterGanNingHappyGodEntries(Func<EngineOwners.CharacterGanNingHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGanNingHappyGodEntries Create(Func<EngineOwners.CharacterGanNingHappyGodRuntime> resolve) => new(resolve);
}
