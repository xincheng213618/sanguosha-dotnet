#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHuanJieMobileShiRuntime))]
internal sealed partial class CharacterHuanJieMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanJieMobileShiRuntime> _resolve;
    private CharacterHuanJieMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanJieMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuanJieMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuanJieMobileShiRuntime> resolve) => new(resolve);
}
