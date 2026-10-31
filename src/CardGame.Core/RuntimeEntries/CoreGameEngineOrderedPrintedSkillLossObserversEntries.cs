#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOrderedPrintedSkillLossObserversRuntime))]
internal sealed partial class CoreGameEngineOrderedPrintedSkillLossObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOrderedPrintedSkillLossObserversRuntime> _resolve;
    private CoreGameEngineOrderedPrintedSkillLossObserversEntries(Func<EngineOwners.CoreGameEngineOrderedPrintedSkillLossObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOrderedPrintedSkillLossObserversEntries Create(Func<EngineOwners.CoreGameEngineOrderedPrintedSkillLossObserversRuntime> resolve) => new(resolve);
}
