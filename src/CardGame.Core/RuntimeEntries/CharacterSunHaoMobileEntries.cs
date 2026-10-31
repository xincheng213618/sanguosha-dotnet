#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunHaoMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHaoMobileRuntime> _resolve;
    private CharacterSunHaoMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHaoMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunHaoMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunHaoMobileRuntime> resolve) => new(resolve);
}
