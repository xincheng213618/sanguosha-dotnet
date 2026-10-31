#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterManChongBoundaryMobileRuntime))]
internal sealed partial class CharacterManChongBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterManChongBoundaryMobileRuntime> _resolve;
    private CharacterManChongBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterManChongBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterManChongBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterManChongBoundaryMobileRuntime> resolve) => new(resolve);
}
