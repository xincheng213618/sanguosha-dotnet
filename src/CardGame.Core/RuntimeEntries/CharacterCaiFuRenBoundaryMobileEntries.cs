#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaiFuRenBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiFuRenBoundaryMobileRuntime> _resolve;
    private CharacterCaiFuRenBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiFuRenBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaiFuRenBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaiFuRenBoundaryMobileRuntime> resolve) => new(resolve);
}
