#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramJudgmentProducer(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeJudgmentEntries> getCoreNativeJudgmentRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries> getCoreActiveNativeAttackQueriesRuntime) : IProgramJudgmentProducer
{
    public void Begin(long programFrameId, int targetSeat, string reason, CardKind? sourceCard, int sourceSeat, string resultBind, SkillProgramCardSetVisibility visibility) => _ = getCoreNativeJudgmentRuntime().BeginJudgment(attack: getCoreActiveNativeAttackQueriesRuntime().ActiveCardAttack, targetSeat, reason, programFrameId, sourceCard: sourceCard, JudgmentContinuationKind.ProgramSkill, sourceSeat: sourceSeat, programResultBind: resultBind, programResultVisibility: visibility);
}
