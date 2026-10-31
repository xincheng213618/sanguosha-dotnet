#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXingDaoRongAnniversaryEntries
{
    internal bool AdvanceXingDaoRongObserver() => _resolve().AdvanceXingDaoRongObserver();
    internal void AnswerXingDaoRong(PromptChoice answer) => _resolve().AnswerXingDaoRong(answer);
    internal SkillProgramStepOutcome BeginXingDaoRong(ProgramSkillFrame supplied) => _resolve().BeginXingDaoRong(supplied);
    internal bool ContinueXingDaoRong(long id) => _resolve().ContinueXingDaoRong(id);
    internal bool DrainXingDaoRong(ProgramSkillFrame f) => _resolve().DrainXingDaoRong(f);
    internal bool KeepPaidXingDaoRong(ProgramSkillFrame f) => _resolve().KeepPaidXingDaoRong(f);
    internal void PublishXingDaoRong(ProgramSkillFrame f) => _resolve().PublishXingDaoRong(f);
    internal bool QualifyXingDaoRong(ProgramTriggerLegalityContext g) => _resolve().QualifyXingDaoRong(g);
    internal void ReturnXingDaoRongDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnXingDaoRongDraw(supplied, outcome);
    internal bool ReturnXingDaoRongMovement(ProgramSkillFrame f) => _resolve().ReturnXingDaoRongMovement(f);
    internal bool ValidXingDaoRong(ProgramSkillFrame f) => _resolve().ValidXingDaoRong(f);
    internal PromptChoice XingDaoRongAi(PendingDecision p, ProgramSkillFrame f) => _resolve().XingDaoRongAi(p, f);
    internal IReadOnlyList<PromptChoice> XingDaoRongCards(ProgramSkillFrame f) => _resolve().XingDaoRongCards(f);
    internal IReadOnlyList<PromptChoice> XingDaoRongChoices(ProgramSkillFrame f) => _resolve().XingDaoRongChoices(f);
    internal bool XingDaoRongFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().XingDaoRongFirstChild(f, child);
    internal int[] XingDaoRongNearby(int owner) => _resolve().XingDaoRongNearby(owner);
    internal ProgramSkillFrame? XingDaoRongObserverRoot() => _resolve().XingDaoRongObserverRoot();
    internal bool XingDaoRongObserverSuffix(int i) => _resolve().XingDaoRongObserverSuffix(i);
    internal bool XingDaoRongOriginal(ProgramSkillFrame f) => _resolve().XingDaoRongOriginal(f);
    internal bool XingDaoRongRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame r) => _resolve().XingDaoRongRecoveryParent(f, r);
}
