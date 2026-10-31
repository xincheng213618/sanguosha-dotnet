#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangWenAnniversaryEntries
{
    internal void AddZhangWenSibianStep(long id, global::CardGame.Core.ZhangWenSibianStage stage, int before) => _resolve().AddZhangWenSibianStep(id, stage, before);
    internal bool AdvanceZhangWenSubtree() => _resolve().AdvanceZhangWenSubtree();
    internal void AnswerZhangWenSibian(global::CardGame.Core.PromptChoice answer) => _resolve().AnswerZhangWenSibian(answer);
    internal void AssertZhangWen(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertZhangWen(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginZhangWenSibian(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginZhangWenSibian(supplied);
    internal void CaptureZhangWenSongshu(global::CardGame.Core.ProgramSkillStartedEvent started) => _resolve().CaptureZhangWenSongshu(started);
    internal global::CardGame.Core.PromptChoice ChooseZhangWenSibian(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseZhangWenSibian(p, f);
    internal bool ContinueZhangWenSibian(long id) => _resolve().ContinueZhangWenSibian(id);
    internal bool DrainZhangWenSibian(long id) => _resolve().DrainZhangWenSibian(id);
    internal bool HasAnniversaryZhangWenPrompt() => _resolve().HasAnniversaryZhangWenPrompt();
    internal bool KeepZhangWenAcceptedAfterSkillLoss(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepZhangWenAcceptedAfterSkillLoss(f);
    internal bool KeepZhangWenSibianPayment(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepZhangWenSibianPayment(f);
    internal void MoveZhangWenSibian(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ZhangWenSibianStage stage, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason) => _resolve().MoveZhangWenSibian(f, stage, ids, destination, reason);
    internal void PublishZhangWenSibian(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishZhangWenSibian(f);
    internal void RefundZhangWenSongshu(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().RefundZhangWenSongshu(supplied);
    internal void RequireZhangWenSibian(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireZhangWenSibian(f);
    internal bool ReturnZhangWenSibianMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnZhangWenSibianMovement(f);
    internal bool ValidZhangWenSibian(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidZhangWenSibian(f);
    internal bool ZhangWenBeforeDamageObserver(long id) => _resolve().ZhangWenBeforeDamageObserver(id);
    internal bool ZhangWenDamageObserver(long id) => _resolve().ZhangWenDamageObserver(id);
    internal bool ZhangWenDyingObserver() => _resolve().ZhangWenDyingObserver();
    internal bool ZhangWenEntityAvailable(global::CardGame.Core.ZhangWenSibianEntity entity) => _resolve().ZhangWenEntityAvailable(entity);
    internal bool ZhangWenFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().ZhangWenFirstChild(f, child);
    internal bool ZhangWenMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().ZhangWenMovement(f, e, p);
    internal global::CardGame.Core.ProgramSkillFrame? ZhangWenObserverRoot() => _resolve().ZhangWenObserverRoot();
    internal int[] ZhangWenRemainder(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhangWenRemainder(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ZhangWenSibianChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZhangWenSibianChoices(f);
}
