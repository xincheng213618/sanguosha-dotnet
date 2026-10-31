#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterYanJunOlRuntime))]
internal sealed partial class CharacterYanJunOlEntries
{
    private readonly Func<EngineOwners.CharacterYanJunOlRuntime> _resolve;
    private CharacterYanJunOlEntries(Func<EngineOwners.CharacterYanJunOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYanJunOlEntries Create(Func<EngineOwners.CharacterYanJunOlRuntime> resolve) => new(resolve);
}
