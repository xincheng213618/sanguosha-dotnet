#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime))]
internal sealed partial class CharacterDaQiaoXiaoQiaoHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> _resolve;
    private CharacterDaQiaoXiaoQiaoHappyGodEntries(Func<EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDaQiaoXiaoQiaoHappyGodEntries Create(Func<EngineOwners.CharacterDaQiaoXiaoQiaoHappyGodRuntime> resolve) => new(resolve);
}
