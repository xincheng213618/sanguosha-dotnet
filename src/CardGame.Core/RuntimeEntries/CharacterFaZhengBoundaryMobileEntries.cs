#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterFaZhengBoundaryMobileRuntime))]
internal sealed partial class CharacterFaZhengBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterFaZhengBoundaryMobileRuntime> _resolve;
    private CharacterFaZhengBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFaZhengBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterFaZhengBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterFaZhengBoundaryMobileRuntime> resolve) => new(resolve);
}
