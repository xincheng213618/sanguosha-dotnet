#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhongHuiAnniversaryGodRuntime))]
internal sealed partial class CharacterZhongHuiAnniversaryGodEntries
{
    private readonly Func<EngineOwners.CharacterZhongHuiAnniversaryGodRuntime> _resolve;
    private CharacterZhongHuiAnniversaryGodEntries(Func<EngineOwners.CharacterZhongHuiAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhongHuiAnniversaryGodEntries Create(Func<EngineOwners.CharacterZhongHuiAnniversaryGodRuntime> resolve) => new(resolve);
}
