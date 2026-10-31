#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterSunHongOlRuntime))]
internal sealed partial class CharacterSunHongOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHongOlRuntime> _resolve;
    private CharacterSunHongOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHongOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunHongOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHongOlRuntime> resolve) => new(resolve);
}
