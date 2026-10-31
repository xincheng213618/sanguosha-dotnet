#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSharedPhaseSkillsRuntime))]
internal sealed partial class CoreGameEngineSharedPhaseSkillsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSharedPhaseSkillsRuntime> _resolve;
    private CoreGameEngineSharedPhaseSkillsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSharedPhaseSkillsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSharedPhaseSkillsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSharedPhaseSkillsRuntime> resolve) => new(resolve);
}
