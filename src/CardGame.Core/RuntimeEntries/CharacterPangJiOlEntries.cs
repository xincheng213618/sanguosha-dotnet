#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterPangJiOlRuntime))]
internal sealed partial class CharacterPangJiOlEntries
{
    private readonly Func<EngineOwners.CharacterPangJiOlRuntime> _resolve;
    private CharacterPangJiOlEntries(Func<EngineOwners.CharacterPangJiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPangJiOlEntries Create(Func<EngineOwners.CharacterPangJiOlRuntime> resolve) => new(resolve);
}
