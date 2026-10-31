#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime))]
internal sealed partial class CharacterZhangFeiAnniversaryGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime> _resolve;
    private CharacterZhangFeiAnniversaryGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangFeiAnniversaryGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiAnniversaryGodRuntime> resolve) => new(resolve);
}
