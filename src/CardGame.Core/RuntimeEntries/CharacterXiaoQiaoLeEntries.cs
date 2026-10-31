#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiaoQiaoLeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoLeRuntime> _resolve;
    private CharacterXiaoQiaoLeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiaoQiaoLeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiaoQiaoLeRuntime> resolve) => new(resolve);
}
