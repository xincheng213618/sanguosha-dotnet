#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuRanBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuRanBoundaryMobileRuntime> _resolve;
    private CharacterZhuRanBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuRanBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhuRanBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuRanBoundaryMobileRuntime> resolve) => new(resolve);
}
