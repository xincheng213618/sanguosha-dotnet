#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangLiaoHappyGodRuntime))]
internal sealed partial class CharacterZhangLiaoHappyGodEntries
{
    private readonly Func<EngineOwners.CharacterZhangLiaoHappyGodRuntime> _resolve;
    private CharacterZhangLiaoHappyGodEntries(Func<EngineOwners.CharacterZhangLiaoHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangLiaoHappyGodEntries Create(Func<EngineOwners.CharacterZhangLiaoHappyGodRuntime> resolve) => new(resolve);
}
