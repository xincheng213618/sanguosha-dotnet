#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime))]
internal sealed partial class CharacterHuangZhongAnniversaryGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime> _resolve;
    private CharacterHuangZhongAnniversaryGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangZhongAnniversaryGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangZhongAnniversaryGodRuntime> resolve) => new(resolve);
}
