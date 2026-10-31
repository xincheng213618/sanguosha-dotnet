#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterQiaoRuiAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQiaoRuiAnniversaryRuntime> _resolve;
    private CharacterQiaoRuiAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQiaoRuiAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterQiaoRuiAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQiaoRuiAnniversaryRuntime> resolve) => new(resolve);
}
