#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLuYuShengMobileShiRuntime))]
internal sealed partial class CharacterLuYuShengMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuYuShengMobileShiRuntime> _resolve;
    private CharacterLuYuShengMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuYuShengMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuYuShengMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuYuShengMobileShiRuntime> resolve) => new(resolve);
}
