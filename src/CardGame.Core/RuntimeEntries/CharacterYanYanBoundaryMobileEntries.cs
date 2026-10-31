#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterYanYanBoundaryMobileRuntime))]
internal sealed partial class CharacterYanYanBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanYanBoundaryMobileRuntime> _resolve;
    private CharacterYanYanBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanYanBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYanYanBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYanYanBoundaryMobileRuntime> resolve) => new(resolve);
}
