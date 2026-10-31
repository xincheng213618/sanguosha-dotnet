#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDaQiaoLeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime> _resolve;
    private CharacterDaQiaoLeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDaQiaoLeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoLeRuntime> resolve) => new(resolve);
}
