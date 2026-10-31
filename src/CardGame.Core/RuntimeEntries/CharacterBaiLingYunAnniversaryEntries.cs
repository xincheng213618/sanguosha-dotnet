#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBaiLingYunAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaiLingYunAnniversaryRuntime> _resolve;
    private CharacterBaiLingYunAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaiLingYunAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterBaiLingYunAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBaiLingYunAnniversaryRuntime> resolve) => new(resolve);
}
