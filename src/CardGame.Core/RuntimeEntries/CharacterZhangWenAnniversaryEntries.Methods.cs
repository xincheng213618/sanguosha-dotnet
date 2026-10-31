#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangWenAnniversaryEntries
{
    internal void AddZhangWenSibianStep(long id, ZhangWenSibianStage stage, int before) => _resolve().AddZhangWenSibianStep(id, stage, before);
    internal bool AdvanceZhangWenSubtree() => _resolve().AdvanceZhangWenSubtree();
    internal void AnswerZhangWenSibian(PromptChoice answer) => _resolve().AnswerZhangWenSibian(answer);
    internal SkillProgramStepOutcome BeginZhangWenSibian(ProgramSkillFrame supplied) => _resolve().BeginZhangWenSibian(supplied);
    internal void CaptureZhangWenSongshu(ProgramSkillStartedEvent started) => _resolve().CaptureZhangWenSongshu(started);
    internal PromptChoice ChooseZhangWenSibian(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseZhangWenSibian(p, f);
    internal bool ContinueZhangWenSibian(long id) => _resolve().ContinueZhangWenSibian(id);
    internal bool DrainZhangWenSibian(long id) => _resolve().DrainZhangWenSibian(id);
    internal bool KeepZhangWenAcceptedAfterSkillLoss(ProgramSkillFrame f) => _resolve().KeepZhangWenAcceptedAfterSkillLoss(f);
    internal bool KeepZhangWenSibianPayment(ProgramSkillFrame f) => _resolve().KeepZhangWenSibianPayment(f);
    internal void MoveZhangWenSibian(ProgramSkillFrame f, ZhangWenSibianStage stage, IReadOnlyList<int> ids, CardLocation destination, CardMoveReason reason) => _resolve().MoveZhangWenSibian(f, stage, ids, destination, reason);
    internal void PublishZhangWenSibian(ProgramSkillFrame f) => _resolve().PublishZhangWenSibian(f);
    internal void RefundZhangWenSongshu(ProgramSkillFrame supplied) => _resolve().RefundZhangWenSongshu(supplied);
    internal void RequireZhangWenSibian(ProgramSkillFrame f) => _resolve().RequireZhangWenSibian(f);
    internal bool ReturnZhangWenSibianMovement(ProgramSkillFrame f) => _resolve().ReturnZhangWenSibianMovement(f);
    internal bool ValidZhangWenSibian(ProgramSkillFrame f) => _resolve().ValidZhangWenSibian(f);
    internal bool ZhangWenEntityAvailable(ZhangWenSibianEntity entity) => _resolve().ZhangWenEntityAvailable(entity);
    internal bool ZhangWenFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().ZhangWenFirstChild(f, child);
    internal ProgramSkillFrame? ZhangWenObserverRoot() => _resolve().ZhangWenObserverRoot();
    internal int[] ZhangWenRemainder(ProgramSkillFrame f) => _resolve().ZhangWenRemainder(f);
    internal IReadOnlyList<PromptChoice> ZhangWenSibianChoices(ProgramSkillFrame f) => _resolve().ZhangWenSibianChoices(f);
}
