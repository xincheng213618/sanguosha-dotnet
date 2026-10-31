#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiahouJieAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouJieAnniversaryRuntime> _resolve;
    private CharacterXiahouJieAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouJieAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiahouJieAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouJieAnniversaryRuntime> resolve) => new(resolve);
}
