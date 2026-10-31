#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGanNingHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGanNingHappyGodRuntime> _resolve;
    private CharacterGanNingHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGanNingHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGanNingHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGanNingHappyGodRuntime> resolve) => new(resolve);
}
