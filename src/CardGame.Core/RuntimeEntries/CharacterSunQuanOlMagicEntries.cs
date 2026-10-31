#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime))]
internal sealed partial class CharacterSunQuanOlMagicEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime> _resolve;
    private CharacterSunQuanOlMagicEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunQuanOlMagicEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanOlMagicRuntime> resolve) => new(resolve);
}
