#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiahouHuiAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouHuiAnniversaryRuntime> _resolve;
    private CharacterXiahouHuiAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouHuiAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiahouHuiAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouHuiAnniversaryRuntime> resolve) => new(resolve);
}
