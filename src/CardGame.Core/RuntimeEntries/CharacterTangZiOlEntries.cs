#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterTangZiOlRuntime))]
internal sealed partial class CharacterTangZiOlEntries
{
    private readonly Func<EngineOwners.CharacterTangZiOlRuntime> _resolve;
    private CharacterTangZiOlEntries(Func<EngineOwners.CharacterTangZiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTangZiOlEntries Create(Func<EngineOwners.CharacterTangZiOlRuntime> resolve) => new(resolve);
}
