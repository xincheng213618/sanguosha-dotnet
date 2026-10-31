#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLiuPanOlRuntime))]
internal sealed partial class CharacterLiuPanOlEntries
{
    private readonly Func<EngineOwners.CharacterLiuPanOlRuntime> _resolve;
    private CharacterLiuPanOlEntries(Func<EngineOwners.CharacterLiuPanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuPanOlEntries Create(Func<EngineOwners.CharacterLiuPanOlRuntime> resolve) => new(resolve);
}
