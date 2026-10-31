#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterWuYiBoundaryMobileRuntime))]
internal sealed partial class CharacterWuYiBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuYiBoundaryMobileRuntime> _resolve;
    private CharacterWuYiBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuYiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWuYiBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWuYiBoundaryMobileRuntime> resolve) => new(resolve);
}
