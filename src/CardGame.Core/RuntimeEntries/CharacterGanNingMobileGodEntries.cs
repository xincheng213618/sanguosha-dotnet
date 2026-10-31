#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGanNingMobileGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGanNingMobileGodRuntime> _resolve;
    private CharacterGanNingMobileGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGanNingMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGanNingMobileGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGanNingMobileGodRuntime> resolve) => new(resolve);
}
