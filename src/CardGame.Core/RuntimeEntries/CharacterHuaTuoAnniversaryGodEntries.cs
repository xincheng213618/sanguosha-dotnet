#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHuaTuoAnniversaryGodRuntime))]
internal sealed partial class CharacterHuaTuoAnniversaryGodEntries
{
    private readonly Func<EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> _resolve;
    private CharacterHuaTuoAnniversaryGodEntries(Func<EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuaTuoAnniversaryGodEntries Create(Func<EngineOwners.CharacterHuaTuoAnniversaryGodRuntime> resolve) => new(resolve);
}
