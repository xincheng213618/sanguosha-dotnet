#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterShiXieMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterShiXieMobileRuntime> _resolve;
    private CharacterShiXieMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterShiXieMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterShiXieMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterShiXieMobileRuntime> resolve) => new(resolve);
}
