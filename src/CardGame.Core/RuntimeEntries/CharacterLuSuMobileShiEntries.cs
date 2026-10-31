#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime))]
internal sealed partial class CharacterLuSuMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime> _resolve;
    private CharacterLuSuMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuSuMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuSuMobileShiRuntime> resolve) => new(resolve);
}
