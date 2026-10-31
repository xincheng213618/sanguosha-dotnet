#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterManChongBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterManChongBoundaryMobileRuntime> _resolve;
    private CharacterManChongBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterManChongBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterManChongBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterManChongBoundaryMobileRuntime> resolve) => new(resolve);
}
