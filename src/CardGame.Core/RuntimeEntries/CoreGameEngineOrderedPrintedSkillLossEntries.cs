#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOrderedPrintedSkillLossRuntime))]
internal sealed partial class CoreGameEngineOrderedPrintedSkillLossEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOrderedPrintedSkillLossRuntime> _resolve;
    private CoreGameEngineOrderedPrintedSkillLossEntries(Func<EngineOwners.CoreGameEngineOrderedPrintedSkillLossRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOrderedPrintedSkillLossEntries Create(Func<EngineOwners.CoreGameEngineOrderedPrintedSkillLossRuntime> resolve) => new(resolve);
}
