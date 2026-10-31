#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNoDamageSkillDisableEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNoDamageSkillDisableRuntime> _resolve;
    private CoreGameEngineNoDamageSkillDisableEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNoDamageSkillDisableRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNoDamageSkillDisableEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNoDamageSkillDisableRuntime> resolve) => new(resolve);
}
