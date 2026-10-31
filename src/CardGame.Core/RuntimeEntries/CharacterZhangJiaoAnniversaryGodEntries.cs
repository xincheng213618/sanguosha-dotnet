#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhangJiaoAnniversaryGodRuntime))]
internal sealed partial class CharacterZhangJiaoAnniversaryGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangJiaoAnniversaryGodRuntime> _resolve;
    private CharacterZhangJiaoAnniversaryGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangJiaoAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangJiaoAnniversaryGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangJiaoAnniversaryGodRuntime> resolve) => new(resolve);
}
