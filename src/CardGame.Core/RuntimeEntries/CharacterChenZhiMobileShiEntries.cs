#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterChenZhiMobileShiRuntime))]
internal sealed partial class CharacterChenZhiMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenZhiMobileShiRuntime> _resolve;
    private CharacterChenZhiMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenZhiMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChenZhiMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenZhiMobileShiRuntime> resolve) => new(resolve);
}
