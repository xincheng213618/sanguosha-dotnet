#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterBuLianShiMobileRuntime))]
internal sealed partial class CharacterBuLianShiMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBuLianShiMobileRuntime> _resolve;
    private CharacterBuLianShiMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBuLianShiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterBuLianShiMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBuLianShiMobileRuntime> resolve) => new(resolve);
}
