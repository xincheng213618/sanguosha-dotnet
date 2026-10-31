#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLvDaiAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvDaiAnniversaryRuntime> _resolve;
    private CharacterLvDaiAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvDaiAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLvDaiAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvDaiAnniversaryRuntime> resolve) => new(resolve);
}
