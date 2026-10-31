#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterXingDaoRongAnniversaryRuntime))]
internal sealed partial class CharacterXingDaoRongAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXingDaoRongAnniversaryRuntime> _resolve;
    private CharacterXingDaoRongAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXingDaoRongAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXingDaoRongAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXingDaoRongAnniversaryRuntime> resolve) => new(resolve);
}
