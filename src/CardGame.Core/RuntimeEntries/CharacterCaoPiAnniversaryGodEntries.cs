#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaoPiAnniversaryGodRuntime))]
internal sealed partial class CharacterCaoPiAnniversaryGodEntries
{
    private readonly Func<EngineOwners.CharacterCaoPiAnniversaryGodRuntime> _resolve;
    private CharacterCaoPiAnniversaryGodEntries(Func<EngineOwners.CharacterCaoPiAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoPiAnniversaryGodEntries Create(Func<EngineOwners.CharacterCaoPiAnniversaryGodRuntime> resolve) => new(resolve);
}
