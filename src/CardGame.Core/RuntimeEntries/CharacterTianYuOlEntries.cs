#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTianYuOlEntries
{
    private readonly Func<EngineOwners.CharacterTianYuOlRuntime> _resolve;
    private CharacterTianYuOlEntries(Func<EngineOwners.CharacterTianYuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTianYuOlEntries Create(Func<EngineOwners.CharacterTianYuOlRuntime> resolve) => new(resolve);
}
