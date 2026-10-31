#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuangZhongAnniversaryGodRuntime))]
internal sealed partial class CharacterHuangZhongAnniversaryGodEntries
{
    private readonly Func<EngineOwners.CharacterHuangZhongAnniversaryGodRuntime> _resolve;
    private CharacterHuangZhongAnniversaryGodEntries(Func<EngineOwners.CharacterHuangZhongAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangZhongAnniversaryGodEntries Create(Func<EngineOwners.CharacterHuangZhongAnniversaryGodRuntime> resolve) => new(resolve);
}
