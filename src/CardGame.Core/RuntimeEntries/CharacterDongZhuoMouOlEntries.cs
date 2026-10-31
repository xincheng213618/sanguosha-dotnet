#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDongZhuoMouOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongZhuoMouOlRuntime> _resolve;
    private CharacterDongZhuoMouOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongZhuoMouOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDongZhuoMouOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDongZhuoMouOlRuntime> resolve) => new(resolve);
}
