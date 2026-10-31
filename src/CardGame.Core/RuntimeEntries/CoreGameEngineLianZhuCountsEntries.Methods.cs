#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLianZhuCountsEntries
{
    internal bool CanDeliverLianZhuGift(int ownerSeat, int recipientSeat, int cardId) => _resolve().CanDeliverLianZhuGift(ownerSeat, recipientSeat, cardId);
    internal PromptChoice ChooseLianZhuAiChoice(ProgramSkillFrame frame) => _resolve().ChooseLianZhuAiChoice(frame);
    internal ProgramSkillFrame FreezeLianZhuChoiceAmount(ProgramSkillFrame frame) => _resolve().FreezeLianZhuChoiceAmount(frame);
    internal SkillProgramEffect LianZhuPausedEffect(ProgramSkillFrame frame) => _resolve().LianZhuPausedEffect(frame);
    internal void RequireLianZhuChoiceAmount(ProgramSkillFrame frame, LianZhuReceipt r) => _resolve().RequireLianZhuChoiceAmount(frame, r);
    internal void RequireNativeLianZhuChoiceAmount(LianZhuChoiceAmountCapturedEvent captured) => _resolve().RequireNativeLianZhuChoiceAmount(captured);
    internal bool UsesRecipientHpLianZhu(ProgramSkillFrame frame) => _resolve().UsesRecipientHpLianZhu(frame);
}
