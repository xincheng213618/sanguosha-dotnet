#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuShanBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuShanBoundaryMobileRuntime> _resolve;
    private CharacterLiuShanBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuShanBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuShanBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuShanBoundaryMobileRuntime> resolve) => new(resolve);
}
