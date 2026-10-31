#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiRuBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiRuBoundaryMobileRuntime> _resolve;
    private CharacterLiRuBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiRuBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiRuBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiRuBoundaryMobileRuntime> resolve) => new(resolve);
}
