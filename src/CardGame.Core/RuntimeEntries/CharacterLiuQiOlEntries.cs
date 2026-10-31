#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLiuQiOlRuntime))]
internal sealed partial class CharacterLiuQiOlEntries
{
    private readonly Func<EngineOwners.CharacterLiuQiOlRuntime> _resolve;
    private CharacterLiuQiOlEntries(Func<EngineOwners.CharacterLiuQiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuQiOlEntries Create(Func<EngineOwners.CharacterLiuQiOlRuntime> resolve) => new(resolve);
}
