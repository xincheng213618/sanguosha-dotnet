#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhangRenMobileShiRuntime))]
internal sealed partial class CharacterZhangRenMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangRenMobileShiRuntime> _resolve;
    private CharacterZhangRenMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangRenMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangRenMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangRenMobileShiRuntime> resolve) => new(resolve);
}
