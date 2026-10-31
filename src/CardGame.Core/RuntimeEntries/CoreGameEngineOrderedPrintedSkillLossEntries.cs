#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOrderedPrintedSkillLossRuntime))]
internal sealed partial class CoreGameEngineOrderedPrintedSkillLossEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOrderedPrintedSkillLossRuntime> _resolve;
    private CoreGameEngineOrderedPrintedSkillLossEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOrderedPrintedSkillLossRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOrderedPrintedSkillLossEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOrderedPrintedSkillLossRuntime> resolve) => new(resolve);
}
