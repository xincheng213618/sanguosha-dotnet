#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterMaChaoAnniversaryGodRuntime))]
internal sealed partial class CharacterMaChaoAnniversaryGodEntries
{
    private readonly Func<EngineOwners.CharacterMaChaoAnniversaryGodRuntime> _resolve;
    private CharacterMaChaoAnniversaryGodEntries(Func<EngineOwners.CharacterMaChaoAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaChaoAnniversaryGodEntries Create(Func<EngineOwners.CharacterMaChaoAnniversaryGodRuntime> resolve) => new(resolve);
}
