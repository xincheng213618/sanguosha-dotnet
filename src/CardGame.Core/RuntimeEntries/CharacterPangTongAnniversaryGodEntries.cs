#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterPangTongAnniversaryGodRuntime))]
internal sealed partial class CharacterPangTongAnniversaryGodEntries
{
    private readonly Func<EngineOwners.CharacterPangTongAnniversaryGodRuntime> _resolve;
    private CharacterPangTongAnniversaryGodEntries(Func<EngineOwners.CharacterPangTongAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPangTongAnniversaryGodEntries Create(Func<EngineOwners.CharacterPangTongAnniversaryGodRuntime> resolve) => new(resolve);
}
