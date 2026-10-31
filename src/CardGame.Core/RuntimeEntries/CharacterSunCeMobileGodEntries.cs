#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSunCeMobileGodRuntime))]
internal sealed partial class CharacterSunCeMobileGodEntries
{
    private readonly Func<EngineOwners.CharacterSunCeMobileGodRuntime> _resolve;
    private CharacterSunCeMobileGodEntries(Func<EngineOwners.CharacterSunCeMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunCeMobileGodEntries Create(Func<EngineOwners.CharacterSunCeMobileGodRuntime> resolve) => new(resolve);
}
