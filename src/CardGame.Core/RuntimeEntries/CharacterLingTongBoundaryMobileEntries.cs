#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLingTongBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLingTongBoundaryMobileRuntime> _resolve;
    private CharacterLingTongBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLingTongBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLingTongBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLingTongBoundaryMobileRuntime> resolve) => new(resolve);
}
