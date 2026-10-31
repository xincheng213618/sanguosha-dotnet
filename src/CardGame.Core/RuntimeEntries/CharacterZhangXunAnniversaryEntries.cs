#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangXunAnniversaryRuntime))]
internal sealed partial class CharacterZhangXunAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterZhangXunAnniversaryRuntime> _resolve;
    private CharacterZhangXunAnniversaryEntries(Func<EngineOwners.CharacterZhangXunAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangXunAnniversaryEntries Create(Func<EngineOwners.CharacterZhangXunAnniversaryRuntime> resolve) => new(resolve);
}
