#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreUnconvertedCardUseQualificationEntries
{
    internal bool IsUnconvertedActualCardUse(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.ProgramCardContinuation? continuation) => _resolve().IsUnconvertedActualCardUse(action, window, continuation);
}
