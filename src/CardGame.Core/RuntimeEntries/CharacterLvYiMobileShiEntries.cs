#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterLvYiMobileShiRuntime))]
internal sealed partial class CharacterLvYiMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvYiMobileShiRuntime> _resolve;
    private CharacterLvYiMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvYiMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLvYiMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLvYiMobileShiRuntime> resolve) => new(resolve);
}
