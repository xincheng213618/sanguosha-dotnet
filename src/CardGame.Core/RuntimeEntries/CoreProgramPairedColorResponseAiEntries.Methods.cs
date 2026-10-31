#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreProgramPairedColorResponseAiEntries
{
    internal double GetPairedColorDispositionAiScore(SkillProgramTrigger trigger, int owner, ProgramCardTriggerWindowFrame window) => _resolve().GetPairedColorDispositionAiScore(trigger, owner, window);
}
