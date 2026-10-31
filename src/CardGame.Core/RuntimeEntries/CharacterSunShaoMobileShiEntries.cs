#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterSunShaoMobileShiRuntime))]
internal sealed partial class CharacterSunShaoMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunShaoMobileShiRuntime> _resolve;
    private CharacterSunShaoMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunShaoMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunShaoMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunShaoMobileShiRuntime> resolve) => new(resolve);
}
