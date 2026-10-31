#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoShuangOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoShuangOlRuntime> _resolve;
    private CharacterCaoShuangOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoShuangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoShuangOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoShuangOlRuntime> resolve) => new(resolve);
}
