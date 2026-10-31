#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYanYanBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanYanBoundaryMobileRuntime> _resolve;
    private CharacterYanYanBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanYanBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYanYanBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanYanBoundaryMobileRuntime> resolve) => new(resolve);
}
