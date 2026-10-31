#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterQuHuangOlRuntime))]
internal sealed partial class CharacterQuHuangOlEntries
{
    private readonly Func<EngineOwners.CharacterQuHuangOlRuntime> _resolve;
    private CharacterQuHuangOlEntries(Func<EngineOwners.CharacterQuHuangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterQuHuangOlEntries Create(Func<EngineOwners.CharacterQuHuangOlRuntime> resolve) => new(resolve);
}
