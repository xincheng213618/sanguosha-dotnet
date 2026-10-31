#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGanNingMobileGodEntries
{
    private readonly Func<EngineOwners.CharacterGanNingMobileGodRuntime> _resolve;
    private CharacterGanNingMobileGodEntries(Func<EngineOwners.CharacterGanNingMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGanNingMobileGodEntries Create(Func<EngineOwners.CharacterGanNingMobileGodRuntime> resolve) => new(resolve);
}
