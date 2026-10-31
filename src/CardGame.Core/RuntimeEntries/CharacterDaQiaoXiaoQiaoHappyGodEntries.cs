#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime))]
internal sealed partial class CharacterDaQiaoXiaoQiaoHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> _resolve;
    private CharacterDaQiaoXiaoQiaoHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDaQiaoXiaoQiaoHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> resolve) => new(resolve);
}
