#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterSunCeBoundaryMobileRuntime))]
internal sealed partial class CharacterSunCeBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunCeBoundaryMobileRuntime> _resolve;
    private CharacterSunCeBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunCeBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunCeBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunCeBoundaryMobileRuntime> resolve) => new(resolve);
}
