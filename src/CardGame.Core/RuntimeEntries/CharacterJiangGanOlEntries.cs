#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiangGanOlEntries
{
    private readonly Func<EngineOwners.CharacterJiangGanOlRuntime> _resolve;
    private CharacterJiangGanOlEntries(Func<EngineOwners.CharacterJiangGanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJiangGanOlEntries Create(Func<EngineOwners.CharacterJiangGanOlRuntime> resolve) => new(resolve);
}
