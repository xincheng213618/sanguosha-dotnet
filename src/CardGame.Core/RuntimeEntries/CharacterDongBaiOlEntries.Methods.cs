#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDongBaiOlEntries
{
    internal SkillProgramStepOutcome BeginLianZhu(ProgramSkillFrame supplied, int recipient) => _resolve().BeginLianZhu(supplied, recipient);
    internal bool CanContinueLianZhu(ProgramSkillFrame f) => _resolve().CanContinueLianZhu(f);
    internal void CleanupDongBaiRecipientDeath(int seat) => _resolve().CleanupDongBaiRecipientDeath(seat);
    internal void ContinueLianzhuAfterPublicReveal(ProgramSkillFrame f) => _resolve().ContinueLianzhuAfterPublicReveal(f);
    internal bool DrainLianZhu(ProgramSkillFrame f) => _resolve().DrainLianZhu(f);
    internal void FinishLianZhu(ProgramSkillFrame f) => _resolve().FinishLianZhu(f);
    internal bool IsXiaHuiDiscardForbidden(int actorSeat, Card card, CardLocation from, OwnedCardMoveIntent intent) => _resolve().IsXiaHuiDiscardForbidden(actorSeat, card, from, intent);
    internal bool IsXiaHuiHandLimitExempt(int owner, Card card) => _resolve().IsXiaHuiHandLimitExempt(owner, card);
    internal bool IsXiaHuiPhysicalRestricted(int actorSeat, int cardId) => _resolve().IsXiaHuiPhysicalRestricted(actorSeat, cardId);
    internal void IssueLianZhuSegment(long id, LianZhuReceipt r, int actor) => _resolve().IssueLianZhuSegment(id, r, actor);
    internal PromptChoice LianZhuAiChoice(ProgramSkillFrame f) => _resolve().LianZhuAiChoice(f);
    internal PromptChoice LianZhuChoice(ProgramSkillFrame f, string branch, string text, IReadOnlyList<int>? cards = default) => _resolve().LianZhuChoice(f, branch, text, cards);
    internal IReadOnlyList<PromptChoice> LianZhuChoices(ProgramSkillFrame f) => _resolve().LianZhuChoices(f);
    internal IReadOnlyList<PromptChoice> LianZhuDiscardOptions(ProgramSkillFrame f) => _resolve().LianZhuDiscardOptions(f);
    internal bool LianZhuFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().LianZhuFirstChild(root, child);
    internal ProgramSkillFrame? LianZhuObserverRoot() => _resolve().LianZhuObserverRoot();
    internal void ObserveDongBaiHpDecreased(int seat, int before, int after) => _resolve().ObserveDongBaiHpDecreased(seat, before, after);
    internal void ObserveDongBaiMovement(CardMovementRecord movement, IReadOnlyList<XiaHuiMovementSource>? originalSources = default) => _resolve().ObserveDongBaiMovement(movement, originalSources);
    internal void ObserveDongBaiRecipientDeath(PlayerDiedEvent died) => _resolve().ObserveDongBaiRecipientDeath(died);
    internal void ObserveMovementBatchDongBai(CardMovementBatchBuilder batch, CardMovementRecord movement) => _resolve().ObserveMovementBatchDongBai(batch, movement);
    internal void PayLianZhuDraw(ProgramSkillFrame f) => _resolve().PayLianZhuDraw(f);
    internal void PublishLianZhuChoice(ProgramSkillFrame f) => _resolve().PublishLianZhuChoice(f);
    internal void RequireLianZhu(ProgramSkillFrame f) => _resolve().RequireLianZhu(f);
    internal void ResolveLianZhuChoice(PromptChoice choice) => _resolve().ResolveLianZhuChoice(choice);
    internal bool ResumeLianZhu(long id) => _resolve().ResumeLianZhu(id);
    internal bool ReturnLianZhuMovement(ProgramSkillFrame f) => _resolve().ReturnLianZhuMovement(f);
    internal bool TryAdvanceLianZhuSubtree() => _resolve().TryAdvanceLianZhuSubtree();
}
