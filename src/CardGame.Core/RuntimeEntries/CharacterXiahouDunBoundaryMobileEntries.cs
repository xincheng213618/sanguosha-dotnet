#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterXiahouDunBoundaryMobileRuntime))]
internal sealed partial class CharacterXiahouDunBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouDunBoundaryMobileRuntime> _resolve;
    private CharacterXiahouDunBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouDunBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXiahouDunBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterXiahouDunBoundaryMobileRuntime> resolve) => new(resolve);
}
