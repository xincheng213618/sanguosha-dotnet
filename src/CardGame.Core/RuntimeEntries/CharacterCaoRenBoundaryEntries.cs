#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterCaoRenBoundaryRuntime))]
internal sealed partial class CharacterCaoRenBoundaryEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoRenBoundaryRuntime> _resolve;
    private CharacterCaoRenBoundaryEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoRenBoundaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoRenBoundaryEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterCaoRenBoundaryRuntime> resolve) => new(resolve);
}
