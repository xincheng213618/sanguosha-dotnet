#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWushuangOriginalSkillChoicesEntries
{
    internal void ResolveWushuangOriginalChoice(PromptChoice choice) => _resolve().ResolveWushuangOriginalChoice(choice);
    internal PromptChoice SelectWushuangOriginalAi(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectWushuangOriginalAi(decision, f);
}
