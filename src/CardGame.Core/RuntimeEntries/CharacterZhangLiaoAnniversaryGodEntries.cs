#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhangLiaoAnniversaryGodRuntime))]
internal sealed partial class CharacterZhangLiaoAnniversaryGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoAnniversaryGodRuntime> _resolve;
    private CharacterZhangLiaoAnniversaryGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangLiaoAnniversaryGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoAnniversaryGodRuntime> resolve) => new(resolve);
}
