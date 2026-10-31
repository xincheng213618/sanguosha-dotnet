#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSuFeiOlRuntime))]
internal sealed partial class CharacterSuFeiOlEntries
{
    private readonly Func<EngineOwners.CharacterSuFeiOlRuntime> _resolve;
    private CharacterSuFeiOlEntries(Func<EngineOwners.CharacterSuFeiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSuFeiOlEntries Create(Func<EngineOwners.CharacterSuFeiOlRuntime> resolve) => new(resolve);
}
