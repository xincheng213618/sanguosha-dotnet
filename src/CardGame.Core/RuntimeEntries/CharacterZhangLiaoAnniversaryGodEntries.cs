#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangLiaoAnniversaryGodRuntime))]
internal sealed partial class CharacterZhangLiaoAnniversaryGodEntries
{
    private readonly Func<EngineOwners.CharacterZhangLiaoAnniversaryGodRuntime> _resolve;
    private CharacterZhangLiaoAnniversaryGodEntries(Func<EngineOwners.CharacterZhangLiaoAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangLiaoAnniversaryGodEntries Create(Func<EngineOwners.CharacterZhangLiaoAnniversaryGodRuntime> resolve) => new(resolve);
}
