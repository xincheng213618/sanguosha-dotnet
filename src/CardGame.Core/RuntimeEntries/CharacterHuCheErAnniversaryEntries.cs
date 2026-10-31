#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuCheErAnniversaryRuntime))]
internal sealed partial class CharacterHuCheErAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterHuCheErAnniversaryRuntime> _resolve;
    private CharacterHuCheErAnniversaryEntries(Func<EngineOwners.CharacterHuCheErAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuCheErAnniversaryEntries Create(Func<EngineOwners.CharacterHuCheErAnniversaryRuntime> resolve) => new(resolve);
}
