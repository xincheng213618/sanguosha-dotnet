#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterGuYongBoundaryMobileRuntime))]
internal sealed partial class CharacterGuYongBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuYongBoundaryMobileRuntime> _resolve;
    private CharacterGuYongBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuYongBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuYongBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuYongBoundaryMobileRuntime> resolve) => new(resolve);
}
