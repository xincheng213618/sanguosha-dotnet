#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDirectSkillDamageRuntime))]
internal sealed partial class CoreGameEngineDirectSkillDamageEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectSkillDamageRuntime> _resolve;
    private CoreGameEngineDirectSkillDamageEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectSkillDamageRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDirectSkillDamageEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDirectSkillDamageRuntime> resolve) => new(resolve);
}
