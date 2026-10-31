#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime))]
internal sealed partial class CharacterZhangLiaoMobileGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime> _resolve;
    private CharacterZhangLiaoMobileGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangLiaoMobileGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangLiaoMobileGodRuntime> resolve) => new(resolve);
}
