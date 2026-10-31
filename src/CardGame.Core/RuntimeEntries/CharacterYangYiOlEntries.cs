#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterYangYiOlRuntime))]
internal sealed partial class CharacterYangYiOlEntries
{
    private readonly Func<EngineOwners.CharacterYangYiOlRuntime> _resolve;
    private CharacterYangYiOlEntries(Func<EngineOwners.CharacterYangYiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYangYiOlEntries Create(Func<EngineOwners.CharacterYangYiOlRuntime> resolve) => new(resolve);
}
