#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGaoXiangAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGaoXiangAnniversaryRuntime> _resolve;
    private CharacterGaoXiangAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGaoXiangAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGaoXiangAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGaoXiangAnniversaryRuntime> resolve) => new(resolve);
}
