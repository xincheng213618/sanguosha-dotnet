#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDongBaiOlEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginLianZhu(global::CardGame.Core.ProgramSkillFrame supplied, int recipient) => _resolve().BeginLianZhu(supplied, recipient);
    internal bool CanContinueLianZhu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueLianZhu(f);
    internal void CleanupDongBaiRecipientDeath(int seat) => _resolve().CleanupDongBaiRecipientDeath(seat);
    internal void ContinueLianzhuAfterPublicReveal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueLianzhuAfterPublicReveal(f);
    internal bool DrainLianZhu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainLianZhu(f);
    internal void FinishLianZhu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishLianZhu(f);
    internal bool IsXiaHuiDiscardForbidden(int actorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent) => _resolve().IsXiaHuiDiscardForbidden(actorSeat, card, from, intent);
    internal bool IsXiaHuiHandLimitExempt(int owner, global::CardGame.Core.Card card) => _resolve().IsXiaHuiHandLimitExempt(owner, card);
    internal bool IsXiaHuiPhysicalRestricted(int actorSeat, int cardId) => _resolve().IsXiaHuiPhysicalRestricted(actorSeat, cardId);
    internal void IssueLianZhuSegment(long id, global::CardGame.Core.LianZhuReceipt r, int actor) => _resolve().IssueLianZhuSegment(id, r, actor);
    internal global::CardGame.Core.PromptChoice LianZhuAiChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LianZhuAiChoice(f);
    internal global::CardGame.Core.PromptChoice LianZhuChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string text, global::System.Collections.Generic.IReadOnlyList<int>? cards = default) => _resolve().LianZhuChoice(f, branch, text, cards);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> LianZhuChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LianZhuChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> LianZhuDiscardOptions(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LianZhuDiscardOptions(f);
    internal bool LianZhuFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().LianZhuFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? LianZhuObserverRoot() => _resolve().LianZhuObserverRoot();
    internal void ObserveDongBaiHpDecreased(int seat, int before, int after) => _resolve().ObserveDongBaiHpDecreased(seat, before, after);
    internal void ObserveDongBaiMovement(global::CardGame.Core.CardMovementRecord movement, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.XiaHuiMovementSource>? originalSources = default) => _resolve().ObserveDongBaiMovement(movement, originalSources);
    internal void ObserveDongBaiRecipientDeath(global::CardGame.Core.PlayerDiedEvent died) => _resolve().ObserveDongBaiRecipientDeath(died);
    internal void ObserveMovementBatchDongBai(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementRecord movement) => _resolve().ObserveMovementBatchDongBai(batch, movement);
    internal void PayLianZhuDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PayLianZhuDraw(f);
    internal void PublishLianZhuChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishLianZhuChoice(f);
    internal void RequireLianZhu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireLianZhu(f);
    internal void ResolveLianZhuChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveLianZhuChoice(choice);
    internal bool ResumeLianZhu(long id) => _resolve().ResumeLianZhu(id);
    internal bool ReturnLianZhuMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnLianZhuMovement(f);
    internal bool TryAdvanceLianZhuSubtree() => _resolve().TryAdvanceLianZhuSubtree();
}
