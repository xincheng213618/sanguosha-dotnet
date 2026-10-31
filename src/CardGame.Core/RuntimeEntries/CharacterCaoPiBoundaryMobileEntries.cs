#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterCaoPiBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiBoundaryMobileRuntime> _resolve;
    private CharacterCaoPiBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoPiBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoPiBoundaryMobileRuntime> resolve) => new(resolve);
}
