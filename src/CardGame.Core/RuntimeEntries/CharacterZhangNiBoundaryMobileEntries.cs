#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhangNiBoundaryMobileRuntime))]
internal sealed partial class CharacterZhangNiBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangNiBoundaryMobileRuntime> _resolve;
    private CharacterZhangNiBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangNiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangNiBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangNiBoundaryMobileRuntime> resolve) => new(resolve);
}
