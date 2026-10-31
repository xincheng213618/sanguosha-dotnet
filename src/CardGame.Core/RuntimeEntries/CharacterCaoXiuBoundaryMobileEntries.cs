#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoXiuBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiuBoundaryMobileRuntime> _resolve;
    private CharacterCaoXiuBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiuBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoXiuBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoXiuBoundaryMobileRuntime> resolve) => new(resolve);
}
