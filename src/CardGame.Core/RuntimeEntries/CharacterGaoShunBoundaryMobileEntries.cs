#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGaoShunBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGaoShunBoundaryMobileRuntime> _resolve;
    private CharacterGaoShunBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGaoShunBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGaoShunBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGaoShunBoundaryMobileRuntime> resolve) => new(resolve);
}
