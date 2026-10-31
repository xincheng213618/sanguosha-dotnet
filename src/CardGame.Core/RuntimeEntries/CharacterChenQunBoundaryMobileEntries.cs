#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenQunBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenQunBoundaryMobileRuntime> _resolve;
    private CharacterChenQunBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenQunBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChenQunBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChenQunBoundaryMobileRuntime> resolve) => new(resolve);
}
