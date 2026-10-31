#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhuRongBoundaryMobileRuntime))]
internal sealed partial class CharacterZhuRongBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuRongBoundaryMobileRuntime> _resolve;
    private CharacterZhuRongBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuRongBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhuRongBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuRongBoundaryMobileRuntime> resolve) => new(resolve);
}
