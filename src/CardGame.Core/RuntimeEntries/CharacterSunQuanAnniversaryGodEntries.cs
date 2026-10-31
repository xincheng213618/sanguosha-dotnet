#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterSunQuanAnniversaryGodRuntime))]
internal sealed partial class CharacterSunQuanAnniversaryGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanAnniversaryGodRuntime> _resolve;
    private CharacterSunQuanAnniversaryGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunQuanAnniversaryGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunQuanAnniversaryGodRuntime> resolve) => new(resolve);
}
