#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterCaoXianOlRuntime))]
internal sealed partial class CharacterCaoXianOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXianOlRuntime> _resolve;
    private CharacterCaoXianOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXianOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoXianOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXianOlRuntime> resolve) => new(resolve);
}
