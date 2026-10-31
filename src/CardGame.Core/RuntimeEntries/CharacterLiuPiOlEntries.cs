#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLiuPiOlRuntime))]
internal sealed partial class CharacterLiuPiOlEntries
{
    private readonly Func<EngineOwners.CharacterLiuPiOlRuntime> _resolve;
    private CharacterLiuPiOlEntries(Func<EngineOwners.CharacterLiuPiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuPiOlEntries Create(Func<EngineOwners.CharacterLiuPiOlRuntime> resolve) => new(resolve);
}
