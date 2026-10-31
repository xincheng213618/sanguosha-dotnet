#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterWuGuoTaiBoundaryMobileRuntime))]
internal sealed partial class CharacterWuGuoTaiBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuGuoTaiBoundaryMobileRuntime> _resolve;
    private CharacterWuGuoTaiBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuGuoTaiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWuGuoTaiBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuGuoTaiBoundaryMobileRuntime> resolve) => new(resolve);
}
