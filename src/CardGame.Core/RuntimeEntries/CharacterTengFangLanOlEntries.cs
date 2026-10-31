#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterTengFangLanOlRuntime))]
internal sealed partial class CharacterTengFangLanOlEntries
{
    private readonly Func<EngineOwners.CharacterTengFangLanOlRuntime> _resolve;
    private CharacterTengFangLanOlEntries(Func<EngineOwners.CharacterTengFangLanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTengFangLanOlEntries Create(Func<EngineOwners.CharacterTengFangLanOlRuntime> resolve) => new(resolve);
}
