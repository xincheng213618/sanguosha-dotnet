#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDengAiBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiBoundaryMobileRuntime> _resolve;
    private CharacterDengAiBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDengAiBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiBoundaryMobileRuntime> resolve) => new(resolve);
}
