
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_gan_ning_mou_mobile_MouGanNingMobile
{
    internal static CardConversionSource MouGnSource(ProgramSkillFrame f) => new(f.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);

    internal static string MouGnPileInstance(long id, int seat) => $"mou-gan-ning:{id}:{seat}";
}
