#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime))]
internal sealed partial class CharacterZhongHuiAnniversaryGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime> _resolve;
    private CharacterZhongHuiAnniversaryGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhongHuiAnniversaryGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime> resolve) => new(resolve);
}
