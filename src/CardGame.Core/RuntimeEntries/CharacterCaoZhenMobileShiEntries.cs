#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterCaoZhenMobileShiRuntime))]
internal sealed partial class CharacterCaoZhenMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhenMobileShiRuntime> _resolve;
    private CharacterCaoZhenMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhenMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoZhenMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoZhenMobileShiRuntime> resolve) => new(resolve);
}
