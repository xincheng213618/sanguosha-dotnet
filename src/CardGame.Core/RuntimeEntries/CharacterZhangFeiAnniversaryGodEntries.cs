#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangFeiAnniversaryGodRuntime))]
internal sealed partial class CharacterZhangFeiAnniversaryGodEntries
{
    private readonly Func<EngineOwners.CharacterZhangFeiAnniversaryGodRuntime> _resolve;
    private CharacterZhangFeiAnniversaryGodEntries(Func<EngineOwners.CharacterZhangFeiAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangFeiAnniversaryGodEntries Create(Func<EngineOwners.CharacterZhangFeiAnniversaryGodRuntime> resolve) => new(resolve);
}
