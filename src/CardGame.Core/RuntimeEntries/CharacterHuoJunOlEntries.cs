#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuoJunOlRuntime))]
internal sealed partial class CharacterHuoJunOlEntries
{
    private readonly Func<EngineOwners.CharacterHuoJunOlRuntime> _resolve;
    private CharacterHuoJunOlEntries(Func<EngineOwners.CharacterHuoJunOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuoJunOlEntries Create(Func<EngineOwners.CharacterHuoJunOlRuntime> resolve) => new(resolve);
}
