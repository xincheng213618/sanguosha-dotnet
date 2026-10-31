#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiaoHuaBoundaryMobileEntries
{
    private readonly Func<EngineOwners.CharacterLiaoHuaBoundaryMobileRuntime> _resolve;
    private CharacterLiaoHuaBoundaryMobileEntries(Func<EngineOwners.CharacterLiaoHuaBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiaoHuaBoundaryMobileEntries Create(Func<EngineOwners.CharacterLiaoHuaBoundaryMobileRuntime> resolve) => new(resolve);
}
