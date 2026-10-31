#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangLiaoMobileGodRuntime))]
internal sealed partial class CharacterZhangLiaoMobileGodEntries
{
    private readonly Func<EngineOwners.CharacterZhangLiaoMobileGodRuntime> _resolve;
    private CharacterZhangLiaoMobileGodEntries(Func<EngineOwners.CharacterZhangLiaoMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangLiaoMobileGodEntries Create(Func<EngineOwners.CharacterZhangLiaoMobileGodRuntime> resolve) => new(resolve);
}
