#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime))]
internal sealed partial class CharacterHuCheErAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime> _resolve;
    private CharacterHuCheErAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuCheErAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuCheErAnniversaryRuntime> resolve) => new(resolve);
}
