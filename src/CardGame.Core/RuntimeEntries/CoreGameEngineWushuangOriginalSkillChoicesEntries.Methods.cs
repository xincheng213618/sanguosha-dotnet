#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangOriginalSkillChoicesEntries
{
    internal void ResolveWushuangOriginalChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveWushuangOriginalChoice(choice);
    internal global::CardGame.Core.PromptChoice SelectWushuangOriginalAi(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectWushuangOriginalAi(decision, f);
}
