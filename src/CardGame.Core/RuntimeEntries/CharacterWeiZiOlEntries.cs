#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWeiZiOlEntries
{
    private readonly Func<EngineOwners.CharacterWeiZiOlRuntime> _resolve;
    private CharacterWeiZiOlEntries(Func<EngineOwners.CharacterWeiZiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWeiZiOlEntries Create(Func<EngineOwners.CharacterWeiZiOlRuntime> resolve) => new(resolve);
}
