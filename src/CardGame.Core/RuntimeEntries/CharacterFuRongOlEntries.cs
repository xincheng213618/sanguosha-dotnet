#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterFuRongOlRuntime))]
internal sealed partial class CharacterFuRongOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuRongOlRuntime> _resolve;
    private CharacterFuRongOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuRongOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterFuRongOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFuRongOlRuntime> resolve) => new(resolve);
}
