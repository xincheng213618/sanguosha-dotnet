#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime))]
internal sealed partial class CharacterGuoSiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime> _resolve;
    private CharacterGuoSiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoSiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoSiOlRuntime> resolve) => new(resolve);
}
