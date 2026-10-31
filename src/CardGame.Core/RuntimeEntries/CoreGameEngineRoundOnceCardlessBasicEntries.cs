#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRoundOnceCardlessBasicRuntime))]
internal sealed partial class CoreGameEngineRoundOnceCardlessBasicEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRoundOnceCardlessBasicRuntime> _resolve;
    private CoreGameEngineRoundOnceCardlessBasicEntries(Func<EngineOwners.CoreGameEngineRoundOnceCardlessBasicRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundOnceCardlessBasicEntries Create(Func<EngineOwners.CoreGameEngineRoundOnceCardlessBasicRuntime> resolve) => new(resolve);
}
