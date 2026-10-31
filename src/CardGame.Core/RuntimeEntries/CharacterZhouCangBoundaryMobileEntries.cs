#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhouCangBoundaryMobileRuntime))]
internal sealed partial class CharacterZhouCangBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouCangBoundaryMobileRuntime> _resolve;
    private CharacterZhouCangBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouCangBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhouCangBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhouCangBoundaryMobileRuntime> resolve) => new(resolve);
}
