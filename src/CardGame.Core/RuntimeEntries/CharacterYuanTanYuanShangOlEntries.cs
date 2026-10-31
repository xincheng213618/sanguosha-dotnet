#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuanTanYuanShangOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanTanYuanShangOlRuntime> _resolve;
    private CharacterYuanTanYuanShangOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanTanYuanShangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYuanTanYuanShangOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanTanYuanShangOlRuntime> resolve) => new(resolve);
}
