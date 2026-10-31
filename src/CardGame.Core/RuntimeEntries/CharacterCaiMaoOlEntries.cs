#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaiMaoOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiMaoOlRuntime> _resolve;
    private CharacterCaiMaoOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiMaoOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaiMaoOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiMaoOlRuntime> resolve) => new(resolve);
}
