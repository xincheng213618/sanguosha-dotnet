#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterTangJiOlRuntime))]
internal sealed partial class CharacterTangJiOlEntries
{
    private readonly Func<EngineOwners.CharacterTangJiOlRuntime> _resolve;
    private CharacterTangJiOlEntries(Func<EngineOwners.CharacterTangJiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTangJiOlEntries Create(Func<EngineOwners.CharacterTangJiOlRuntime> resolve) => new(resolve);
}
