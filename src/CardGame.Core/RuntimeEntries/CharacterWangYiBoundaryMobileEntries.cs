#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterWangYiBoundaryMobileRuntime))]
internal sealed partial class CharacterWangYiBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYiBoundaryMobileRuntime> _resolve;
    private CharacterWangYiBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWangYiBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWangYiBoundaryMobileRuntime> resolve) => new(resolve);
}
