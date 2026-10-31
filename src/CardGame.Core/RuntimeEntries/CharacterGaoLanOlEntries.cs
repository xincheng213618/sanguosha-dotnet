#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGaoLanOlEntries
{
    private readonly Func<EngineOwners.CharacterGaoLanOlRuntime> _resolve;
    private CharacterGaoLanOlEntries(Func<EngineOwners.CharacterGaoLanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGaoLanOlEntries Create(Func<EngineOwners.CharacterGaoLanOlRuntime> resolve) => new(resolve);
}
