#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGaoGanOlEntries
{
    private readonly Func<EngineOwners.CharacterGaoGanOlRuntime> _resolve;
    private CharacterGaoGanOlEntries(Func<EngineOwners.CharacterGaoGanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGaoGanOlEntries Create(Func<EngineOwners.CharacterGaoGanOlRuntime> resolve) => new(resolve);
}
