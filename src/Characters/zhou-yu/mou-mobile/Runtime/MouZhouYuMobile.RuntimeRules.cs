
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_zhou_yu_mou_mobile_MouZhouYuMobile
{
    internal static CardConversionSource MouZyMobileSource(ProgramSkillFrame f) => new(f.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);

    internal static string MouZyMobileSuitName(Suit s) => s switch { Suit.Spade => "黑桃", Suit.Heart => "红桃", Suit.Club => "梅花", Suit.Diamond => "方块", _ => "无花色" };
}
