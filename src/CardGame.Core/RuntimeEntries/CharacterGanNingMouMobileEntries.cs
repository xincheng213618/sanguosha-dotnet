#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGanNingMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGanNingMouMobileRuntime> _resolve;
    private CharacterGanNingMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGanNingMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGanNingMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGanNingMouMobileRuntime> resolve) => new(resolve);
}
