#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSunQuanAnniversaryGodRuntime))]
internal sealed partial class CharacterSunQuanAnniversaryGodEntries
{
    private readonly Func<EngineOwners.CharacterSunQuanAnniversaryGodRuntime> _resolve;
    private CharacterSunQuanAnniversaryGodEntries(Func<EngineOwners.CharacterSunQuanAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunQuanAnniversaryGodEntries Create(Func<EngineOwners.CharacterSunQuanAnniversaryGodRuntime> resolve) => new(resolve);
}
