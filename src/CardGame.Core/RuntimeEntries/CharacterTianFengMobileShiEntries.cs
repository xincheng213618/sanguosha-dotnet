#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterTianFengMobileShiRuntime))]
internal sealed partial class CharacterTianFengMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianFengMobileShiRuntime> _resolve;
    private CharacterTianFengMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianFengMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTianFengMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTianFengMobileShiRuntime> resolve) => new(resolve);
}
