#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLiuZhangOlRuntime))]
internal sealed partial class CharacterLiuZhangOlEntries
{
    private readonly Func<EngineOwners.CharacterLiuZhangOlRuntime> _resolve;
    private CharacterLiuZhangOlEntries(Func<EngineOwners.CharacterLiuZhangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuZhangOlEntries Create(Func<EngineOwners.CharacterLiuZhangOlRuntime> resolve) => new(resolve);
}
