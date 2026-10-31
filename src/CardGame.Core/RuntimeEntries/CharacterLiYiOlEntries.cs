#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLiYiOlRuntime))]
internal sealed partial class CharacterLiYiOlEntries
{
    private readonly Func<EngineOwners.CharacterLiYiOlRuntime> _resolve;
    private CharacterLiYiOlEntries(Func<EngineOwners.CharacterLiYiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiYiOlEntries Create(Func<EngineOwners.CharacterLiYiOlRuntime> resolve) => new(resolve);
}
