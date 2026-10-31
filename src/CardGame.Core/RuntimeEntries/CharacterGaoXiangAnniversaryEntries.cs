#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterGaoXiangAnniversaryRuntime))]
internal sealed partial class CharacterGaoXiangAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterGaoXiangAnniversaryRuntime> _resolve;
    private CharacterGaoXiangAnniversaryEntries(Func<EngineOwners.CharacterGaoXiangAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGaoXiangAnniversaryEntries Create(Func<EngineOwners.CharacterGaoXiangAnniversaryRuntime> resolve) => new(resolve);
}
