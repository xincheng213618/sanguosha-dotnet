#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouFeiBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiBoundaryMobileRuntime> _resolve;
    private CharacterZhouFeiBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouFeiBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouFeiBoundaryMobileRuntime> resolve) => new(resolve);
}
