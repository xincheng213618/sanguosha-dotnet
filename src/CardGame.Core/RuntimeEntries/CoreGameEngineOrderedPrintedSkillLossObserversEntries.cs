#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOrderedPrintedSkillLossObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOrderedPrintedSkillLossObserversRuntime> _resolve;
    private CoreGameEngineOrderedPrintedSkillLossObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOrderedPrintedSkillLossObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOrderedPrintedSkillLossObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOrderedPrintedSkillLossObserversRuntime> resolve) => new(resolve);
}
