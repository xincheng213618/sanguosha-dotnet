#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime))]
internal sealed partial class CoreGameEngineRoundOnceCardlessProgramRequestsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime> _resolve;
    private CoreGameEngineRoundOnceCardlessProgramRequestsEntries(Func<EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundOnceCardlessProgramRequestsEntries Create(Func<EngineOwners.CoreGameEngineRoundOnceCardlessProgramRequestsRuntime> resolve) => new(resolve);
}
