#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangOriginalSkillChoicesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillChoicesRuntime> _resolve;
    private CoreGameEngineWushuangOriginalSkillChoicesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillChoicesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineWushuangOriginalSkillChoicesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangOriginalSkillChoicesRuntime> resolve) => new(resolve);
}
