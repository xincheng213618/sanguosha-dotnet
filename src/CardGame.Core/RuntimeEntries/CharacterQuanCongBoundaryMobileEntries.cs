#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterQuanCongBoundaryMobileRuntime))]
internal sealed partial class CharacterQuanCongBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQuanCongBoundaryMobileRuntime> _resolve;
    private CharacterQuanCongBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQuanCongBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterQuanCongBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQuanCongBoundaryMobileRuntime> resolve) => new(resolve);
}
