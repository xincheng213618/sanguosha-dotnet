#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuBianOlEntries
{
    private readonly Func<EngineOwners.CharacterLiuBianOlRuntime> _resolve;
    private CharacterLiuBianOlEntries(Func<EngineOwners.CharacterLiuBianOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuBianOlEntries Create(Func<EngineOwners.CharacterLiuBianOlRuntime> resolve) => new(resolve);
}
