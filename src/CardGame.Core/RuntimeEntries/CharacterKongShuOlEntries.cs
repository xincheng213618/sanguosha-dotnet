#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterKongShuOlRuntime))]
internal sealed partial class CharacterKongShuOlEntries
{
    private readonly Func<EngineOwners.CharacterKongShuOlRuntime> _resolve;
    private CharacterKongShuOlEntries(Func<EngineOwners.CharacterKongShuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterKongShuOlEntries Create(Func<EngineOwners.CharacterKongShuOlRuntime> resolve) => new(resolve);
}
