#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuangQuanOlRuntime))]
internal sealed partial class CharacterHuangQuanOlEntries
{
    private readonly Func<EngineOwners.CharacterHuangQuanOlRuntime> _resolve;
    private CharacterHuangQuanOlEntries(Func<EngineOwners.CharacterHuangQuanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangQuanOlEntries Create(Func<EngineOwners.CharacterHuangQuanOlRuntime> resolve) => new(resolve);
}
