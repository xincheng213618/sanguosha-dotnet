#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterTangTangOlRuntime))]
internal sealed partial class CharacterTangTangOlEntries
{
    private readonly Func<EngineOwners.CharacterTangTangOlRuntime> _resolve;
    private CharacterTangTangOlEntries(Func<EngineOwners.CharacterTangTangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTangTangOlEntries Create(Func<EngineOwners.CharacterTangTangOlRuntime> resolve) => new(resolve);
}
