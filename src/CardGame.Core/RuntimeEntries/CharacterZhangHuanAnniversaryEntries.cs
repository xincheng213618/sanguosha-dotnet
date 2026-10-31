#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangHuanAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangHuanAnniversaryRuntime> _resolve;
    private CharacterZhangHuanAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangHuanAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangHuanAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangHuanAnniversaryRuntime> resolve) => new(resolve);
}
