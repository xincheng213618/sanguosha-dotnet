#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLvKaiOlRuntime))]
internal sealed partial class CharacterLvKaiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvKaiOlRuntime> _resolve;
    private CharacterLvKaiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvKaiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLvKaiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvKaiOlRuntime> resolve) => new(resolve);
}
