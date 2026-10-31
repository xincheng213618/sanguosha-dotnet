#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhuJunOlRuntime))]
internal sealed partial class CharacterZhuJunOlEntries
{
    private readonly Func<EngineOwners.CharacterZhuJunOlRuntime> _resolve;
    private CharacterZhuJunOlEntries(Func<EngineOwners.CharacterZhuJunOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhuJunOlEntries Create(Func<EngineOwners.CharacterZhuJunOlRuntime> resolve) => new(resolve);
}
