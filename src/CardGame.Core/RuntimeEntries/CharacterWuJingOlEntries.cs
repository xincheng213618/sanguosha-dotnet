#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime))]
internal sealed partial class CharacterWuJingOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> _resolve;
    private CharacterWuJingOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWuJingOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuJingOlRuntime> resolve) => new(resolve);
}
