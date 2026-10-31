#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterSunQuanMobileRuntime))]
internal sealed partial class CharacterSunQuanMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanMobileRuntime> _resolve;
    private CharacterSunQuanMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunQuanMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanMobileRuntime> resolve) => new(resolve);
}
