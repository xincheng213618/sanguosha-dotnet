#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuShanBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterLiuShanBoundaryMobileRuntime> _resolve;
    private CharacterLiuShanBoundaryMobileEntries(Func<EngineOwners.CharacterLiuShanBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuShanBoundaryMobileEntries Create(Func<EngineOwners.CharacterLiuShanBoundaryMobileRuntime> resolve) => new(resolve);
}
