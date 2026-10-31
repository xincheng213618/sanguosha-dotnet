#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSunJianMouOlRuntime))]
internal sealed partial class CharacterSunJianMouOlEntries
{
    private readonly Func<EngineOwners.CharacterSunJianMouOlRuntime> _resolve;
    private CharacterSunJianMouOlEntries(Func<EngineOwners.CharacterSunJianMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunJianMouOlEntries Create(Func<EngineOwners.CharacterSunJianMouOlRuntime> resolve) => new(resolve);
}
