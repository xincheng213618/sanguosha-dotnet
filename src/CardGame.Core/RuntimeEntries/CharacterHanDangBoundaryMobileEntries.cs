#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterHanDangBoundaryMobileRuntime))]
internal sealed partial class CharacterHanDangBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanDangBoundaryMobileRuntime> _resolve;
    private CharacterHanDangBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanDangBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHanDangBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHanDangBoundaryMobileRuntime> resolve) => new(resolve);
}
