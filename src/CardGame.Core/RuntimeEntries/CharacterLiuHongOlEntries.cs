#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLiuHongOlRuntime))]
internal sealed partial class CharacterLiuHongOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuHongOlRuntime> _resolve;
    private CharacterLiuHongOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuHongOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuHongOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuHongOlRuntime> resolve) => new(resolve);
}
