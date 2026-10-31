#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSunHaoOlRuntime))]
internal sealed partial class CharacterSunHaoOlEntries
{
    private readonly Func<EngineOwners.CharacterSunHaoOlRuntime> _resolve;
    private CharacterSunHaoOlEntries(Func<EngineOwners.CharacterSunHaoOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunHaoOlEntries Create(Func<EngineOwners.CharacterSunHaoOlRuntime> resolve) => new(resolve);
}
