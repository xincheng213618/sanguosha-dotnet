#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterZhangFeiBoundaryMobileRuntime))]
internal sealed partial class CharacterZhangFeiBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiBoundaryMobileRuntime> _resolve;
    private CharacterZhangFeiBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangFeiBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangFeiBoundaryMobileRuntime> resolve) => new(resolve);
}
