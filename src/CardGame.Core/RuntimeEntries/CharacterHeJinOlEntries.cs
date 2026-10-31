#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHeJinOlRuntime))]
internal sealed partial class CharacterHeJinOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeJinOlRuntime> _resolve;
    private CharacterHeJinOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeJinOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHeJinOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHeJinOlRuntime> resolve) => new(resolve);
}
