#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterSunHaoOlRuntime))]
internal sealed partial class CharacterSunHaoOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHaoOlRuntime> _resolve;
    private CharacterSunHaoOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHaoOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunHaoOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHaoOlRuntime> resolve) => new(resolve);
}
