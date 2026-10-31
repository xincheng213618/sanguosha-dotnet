#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDianWeiBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDianWeiBoundaryMobileRuntime> _resolve;
    private CharacterDianWeiBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDianWeiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDianWeiBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDianWeiBoundaryMobileRuntime> resolve) => new(resolve);
}
