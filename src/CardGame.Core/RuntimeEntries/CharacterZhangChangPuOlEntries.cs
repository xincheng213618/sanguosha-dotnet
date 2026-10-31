#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangChangPuOlEntries
{
    private readonly Func<EngineOwners.CharacterZhangChangPuOlRuntime> _resolve;
    private CharacterZhangChangPuOlEntries(Func<EngineOwners.CharacterZhangChangPuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangChangPuOlEntries Create(Func<EngineOwners.CharacterZhangChangPuOlRuntime> resolve) => new(resolve);
}
