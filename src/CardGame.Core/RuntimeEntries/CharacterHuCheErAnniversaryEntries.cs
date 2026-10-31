#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuCheErAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime> _resolve;
    private CharacterHuCheErAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuCheErAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime> resolve) => new(resolve);
}
