#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLianZhuCountsEntries
{
    internal bool CanDeliverLianZhuGift(int ownerSeat, int recipientSeat, int cardId) => _resolve().CanDeliverLianZhuGift(ownerSeat, recipientSeat, cardId);
    internal global::CardGame.Core.PromptChoice ChooseLianZhuAiChoice(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ChooseLianZhuAiChoice(frame);
    internal global::CardGame.Core.ProgramSkillFrame FreezeLianZhuChoiceAmount(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FreezeLianZhuChoiceAmount(frame);
    internal global::CardGame.Core.SkillProgramEffect LianZhuPausedEffect(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().LianZhuPausedEffect(frame);
    internal void RequireLianZhuChoiceAmount(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.LianZhuReceipt r) => _resolve().RequireLianZhuChoiceAmount(frame, r);
    internal void RequireNativeLianZhuChoiceAmount(global::CardGame.Core.LianZhuChoiceAmountCapturedEvent captured) => _resolve().RequireNativeLianZhuChoiceAmount(captured);
    internal bool UsesRecipientHpLianZhu(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().UsesRecipientHpLianZhu(frame);
}
