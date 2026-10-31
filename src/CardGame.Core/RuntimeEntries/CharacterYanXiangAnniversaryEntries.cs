#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterYanXiangAnniversaryRuntime))]
internal sealed partial class CharacterYanXiangAnniversaryEntries
{
    private readonly Func<EngineOwners.CharacterYanXiangAnniversaryRuntime> _resolve;
    private CharacterYanXiangAnniversaryEntries(Func<EngineOwners.CharacterYanXiangAnniversaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYanXiangAnniversaryEntries Create(Func<EngineOwners.CharacterYanXiangAnniversaryRuntime> resolve) => new(resolve);
}
