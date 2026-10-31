#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHaoMengAnniversaryRuntime))]
internal sealed partial class CharacterHaoMengAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterHaoMengAnniversaryRuntime> _resolve;
    private CharacterHaoMengAnniversaryEntries(Func<EngineOwners.CharacterHaoMengAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHaoMengAnniversaryEntries Create(Func<EngineOwners.CharacterHaoMengAnniversaryRuntime> resolve) => new(resolve);
}
