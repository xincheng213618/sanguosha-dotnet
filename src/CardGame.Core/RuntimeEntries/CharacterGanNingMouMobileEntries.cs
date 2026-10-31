#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterGanNingMouMobileRuntime))]
internal sealed partial class CharacterGanNingMouMobileEntries
{
    private readonly Func<EngineOwners.CharacterGanNingMouMobileRuntime> _resolve;
    private CharacterGanNingMouMobileEntries(Func<EngineOwners.CharacterGanNingMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGanNingMouMobileEntries Create(Func<EngineOwners.CharacterGanNingMouMobileRuntime> resolve) => new(resolve);
}
