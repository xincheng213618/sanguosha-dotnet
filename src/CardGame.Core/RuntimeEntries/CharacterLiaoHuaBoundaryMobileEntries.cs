#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiaoHuaBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiaoHuaBoundaryMobileRuntime> _resolve;
    private CharacterLiaoHuaBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiaoHuaBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiaoHuaBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiaoHuaBoundaryMobileRuntime> resolve) => new(resolve);
}
