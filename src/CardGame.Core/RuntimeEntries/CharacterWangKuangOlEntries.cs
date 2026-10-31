#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterWangKuangOlRuntime))]
internal sealed partial class CharacterWangKuangOlEntries
{
    private readonly Func<EngineOwners.CharacterWangKuangOlRuntime> _resolve;
    private CharacterWangKuangOlEntries(Func<EngineOwners.CharacterWangKuangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangKuangOlEntries Create(Func<EngineOwners.CharacterWangKuangOlRuntime> resolve) => new(resolve);
}
