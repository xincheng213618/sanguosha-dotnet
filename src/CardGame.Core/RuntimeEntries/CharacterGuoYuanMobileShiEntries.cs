#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime))]
internal sealed partial class CharacterGuoYuanMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime> _resolve;
    private CharacterGuoYuanMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoYuanMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoYuanMobileShiRuntime> resolve) => new(resolve);
}
