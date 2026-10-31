#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHaoMengAnniversaryRuntime))]
internal sealed partial class CharacterHaoMengAnniversaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHaoMengAnniversaryRuntime> _resolve;
    private CharacterHaoMengAnniversaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHaoMengAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHaoMengAnniversaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHaoMengAnniversaryRuntime> resolve) => new(resolve);
}
