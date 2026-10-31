#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYanLiangWenChouBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterYanLiangWenChouBoundaryMobileRuntime> _resolve;
    private CharacterYanLiangWenChouBoundaryMobileEntries(Func<EngineOwners.CharacterYanLiangWenChouBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYanLiangWenChouBoundaryMobileEntries Create(Func<EngineOwners.CharacterYanLiangWenChouBoundaryMobileRuntime> resolve) => new(resolve);
}
