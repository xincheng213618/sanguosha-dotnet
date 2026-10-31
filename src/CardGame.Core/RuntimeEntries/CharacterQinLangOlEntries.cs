#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterQinLangOlRuntime))]
internal sealed partial class CharacterQinLangOlEntries
{
    private readonly Func<EngineOwners.CharacterQinLangOlRuntime> _resolve;
    private CharacterQinLangOlEntries(Func<EngineOwners.CharacterQinLangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterQinLangOlEntries Create(Func<EngineOwners.CharacterQinLangOlRuntime> resolve) => new(resolve);
}
