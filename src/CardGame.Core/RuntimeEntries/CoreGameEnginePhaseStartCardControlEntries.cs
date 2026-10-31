#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePhaseStartCardControlRuntime))]
internal sealed partial class CoreGameEnginePhaseStartCardControlEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePhaseStartCardControlRuntime> _resolve;
    private CoreGameEnginePhaseStartCardControlEntries(Func<EngineOwners.CoreGameEnginePhaseStartCardControlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePhaseStartCardControlEntries Create(Func<EngineOwners.CoreGameEnginePhaseStartCardControlRuntime> resolve) => new(resolve);
}
