#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterWangJiBoundaryMobileRuntime))]
internal sealed partial class CharacterWangJiBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangJiBoundaryMobileRuntime> _resolve;
    private CharacterWangJiBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangJiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangJiBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangJiBoundaryMobileRuntime> resolve) => new(resolve);
}
