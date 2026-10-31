#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterSunCeMobileGodRuntime))]
internal sealed partial class CharacterSunCeMobileGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunCeMobileGodRuntime> _resolve;
    private CharacterSunCeMobileGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunCeMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunCeMobileGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunCeMobileGodRuntime> resolve) => new(resolve);
}
