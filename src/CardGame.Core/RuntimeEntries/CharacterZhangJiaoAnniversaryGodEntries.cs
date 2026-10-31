#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangJiaoAnniversaryGodRuntime))]
internal sealed partial class CharacterZhangJiaoAnniversaryGodEntries
{
    private readonly Func<EngineOwners.CharacterZhangJiaoAnniversaryGodRuntime> _resolve;
    private CharacterZhangJiaoAnniversaryGodEntries(Func<EngineOwners.CharacterZhangJiaoAnniversaryGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangJiaoAnniversaryGodEntries Create(Func<EngineOwners.CharacterZhangJiaoAnniversaryGodRuntime> resolve) => new(resolve);
}
