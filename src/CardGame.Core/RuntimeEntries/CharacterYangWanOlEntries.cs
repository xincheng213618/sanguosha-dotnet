#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterYangWanOlRuntime))]
internal sealed partial class CharacterYangWanOlEntries
{
    private readonly Func<EngineOwners.CharacterYangWanOlRuntime> _resolve;
    private CharacterYangWanOlEntries(Func<EngineOwners.CharacterYangWanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYangWanOlEntries Create(Func<EngineOwners.CharacterYangWanOlRuntime> resolve) => new(resolve);
}
