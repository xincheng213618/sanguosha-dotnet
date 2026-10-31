#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSunHongOlRuntime))]
internal sealed partial class CharacterSunHongOlEntries
{
    private readonly Func<EngineOwners.CharacterSunHongOlRuntime> _resolve;
    private CharacterSunHongOlEntries(Func<EngineOwners.CharacterSunHongOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunHongOlEntries Create(Func<EngineOwners.CharacterSunHongOlRuntime> resolve) => new(resolve);
}
