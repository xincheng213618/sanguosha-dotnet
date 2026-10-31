#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhongYaoMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongYaoMobileRuntime> _resolve;
    private CharacterZhongYaoMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongYaoMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhongYaoMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongYaoMobileRuntime> resolve) => new(resolve);
}
