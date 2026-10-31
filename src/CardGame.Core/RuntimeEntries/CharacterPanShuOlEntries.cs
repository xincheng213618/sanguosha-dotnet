#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPanShuOlEntries
{
    private readonly Func<EngineOwners.CharacterPanShuOlRuntime> _resolve;
    private CharacterPanShuOlEntries(Func<EngineOwners.CharacterPanShuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPanShuOlEntries Create(Func<EngineOwners.CharacterPanShuOlRuntime> resolve) => new(resolve);
}
