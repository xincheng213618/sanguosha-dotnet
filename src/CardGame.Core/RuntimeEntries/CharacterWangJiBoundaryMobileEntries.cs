#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWangJiBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangJiBoundaryMobileRuntime> _resolve;
    private CharacterWangJiBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangJiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangJiBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangJiBoundaryMobileRuntime> resolve) => new(resolve);
}
