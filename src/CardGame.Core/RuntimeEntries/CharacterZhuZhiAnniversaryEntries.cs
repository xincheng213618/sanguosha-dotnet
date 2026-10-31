#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhuZhiAnniversaryRuntime))]
internal sealed partial class CharacterZhuZhiAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterZhuZhiAnniversaryRuntime> _resolve;
    private CharacterZhuZhiAnniversaryEntries(Func<EngineOwners.CharacterZhuZhiAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhuZhiAnniversaryEntries Create(Func<EngineOwners.CharacterZhuZhiAnniversaryRuntime> resolve) => new(resolve);
}
