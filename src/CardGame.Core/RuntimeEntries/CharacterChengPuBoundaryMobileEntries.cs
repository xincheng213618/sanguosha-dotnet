#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChengPuBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengPuBoundaryMobileRuntime> _resolve;
    private CharacterChengPuBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengPuBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChengPuBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterChengPuBoundaryMobileRuntime> resolve) => new(resolve);
}
