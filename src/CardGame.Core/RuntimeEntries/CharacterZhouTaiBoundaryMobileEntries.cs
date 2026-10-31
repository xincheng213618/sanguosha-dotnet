#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouTaiBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouTaiBoundaryMobileRuntime> _resolve;
    private CharacterZhouTaiBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouTaiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouTaiBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouTaiBoundaryMobileRuntime> resolve) => new(resolve);
}
