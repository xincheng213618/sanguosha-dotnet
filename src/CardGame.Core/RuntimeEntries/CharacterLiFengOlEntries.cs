#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime))]
internal sealed partial class CharacterLiFengOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime> _resolve;
    private CharacterLiFengOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiFengOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiFengOlRuntime> resolve) => new(resolve);
}
