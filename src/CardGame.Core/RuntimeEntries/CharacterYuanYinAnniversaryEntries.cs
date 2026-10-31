#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuanYinAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanYinAnniversaryRuntime> _resolve;
    private CharacterYuanYinAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanYinAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYuanYinAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuanYinAnniversaryRuntime> resolve) => new(resolve);
}
