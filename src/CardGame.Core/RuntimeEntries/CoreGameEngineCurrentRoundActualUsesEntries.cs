#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCurrentRoundActualUsesRuntime))]
internal sealed partial class CoreGameEngineCurrentRoundActualUsesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCurrentRoundActualUsesRuntime> _resolve;
    private CoreGameEngineCurrentRoundActualUsesEntries(Func<EngineOwners.CoreGameEngineCurrentRoundActualUsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCurrentRoundActualUsesEntries Create(Func<EngineOwners.CoreGameEngineCurrentRoundActualUsesRuntime> resolve) => new(resolve);
}
