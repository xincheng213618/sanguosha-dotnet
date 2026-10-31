#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuanGongZhuOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuanGongZhuOlRuntime> _resolve;
    private CharacterXuanGongZhuOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuanGongZhuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXuanGongZhuOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXuanGongZhuOlRuntime> resolve) => new(resolve);
}
