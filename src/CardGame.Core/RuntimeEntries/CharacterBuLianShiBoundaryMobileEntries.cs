#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterBuLianShiBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBuLianShiBoundaryMobileRuntime> _resolve;
    private CharacterBuLianShiBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBuLianShiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterBuLianShiBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBuLianShiBoundaryMobileRuntime> resolve) => new(resolve);
}
