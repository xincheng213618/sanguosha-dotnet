#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiangXingOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiangXingOlRuntime> _resolve;
    private CharacterLiangXingOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiangXingOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiangXingOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiangXingOlRuntime> resolve) => new(resolve);
}
