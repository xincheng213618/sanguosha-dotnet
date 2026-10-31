#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDengAiMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiMobileShiRuntime> _resolve;
    private CharacterDengAiMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDengAiMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiMobileShiRuntime> resolve) => new(resolve);
}
