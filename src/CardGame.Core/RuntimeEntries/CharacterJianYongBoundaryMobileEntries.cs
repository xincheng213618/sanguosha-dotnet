#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterJianYongBoundaryMobileRuntime))]
internal sealed partial class CharacterJianYongBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJianYongBoundaryMobileRuntime> _resolve;
    private CharacterJianYongBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJianYongBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJianYongBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJianYongBoundaryMobileRuntime> resolve) => new(resolve);
}
