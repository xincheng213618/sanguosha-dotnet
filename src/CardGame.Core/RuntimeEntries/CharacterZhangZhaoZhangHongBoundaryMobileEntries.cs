#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangZhaoZhangHongBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterZhangZhaoZhangHongBoundaryMobileRuntime> _resolve;
    private CharacterZhangZhaoZhangHongBoundaryMobileEntries(Func<EngineOwners.CharacterZhangZhaoZhangHongBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangZhaoZhangHongBoundaryMobileEntries Create(Func<EngineOwners.CharacterZhangZhaoZhangHongBoundaryMobileRuntime> resolve) => new(resolve);
}
