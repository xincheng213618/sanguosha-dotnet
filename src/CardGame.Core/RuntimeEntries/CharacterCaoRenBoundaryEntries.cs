#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterCaoRenBoundaryRuntime))]
internal sealed partial class CharacterCaoRenBoundaryEntries
{
    private readonly Func<EngineOwners.CharacterCaoRenBoundaryRuntime> _resolve;
    private CharacterCaoRenBoundaryEntries(Func<EngineOwners.CharacterCaoRenBoundaryRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterCaoRenBoundaryEntries Create(Func<EngineOwners.CharacterCaoRenBoundaryRuntime> resolve) => new(resolve);
}
