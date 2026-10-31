#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhangLiaoHappyGodRuntime))]
internal sealed partial class CharacterZhangLiaoHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoHappyGodRuntime> _resolve;
    private CharacterZhangLiaoHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangLiaoHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoHappyGodRuntime> resolve) => new(resolve);
}
