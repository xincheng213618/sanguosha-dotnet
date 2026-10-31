
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_UniqueHpPeerPrograms
{
    internal static string UniqueHpPeerCostReason(ProgramSkillFrame f) => $"skill-program.{f.SkillId}.{SkillProgramEffectOp.DiscardDrawAndOfferUniqueHpPeer}";

    internal static string UniqueHpPeerDrawReason(ProgramSkillFrame f, int seat) => $"skill-program.{f.SkillId}.unique-hp-peer.draw.{seat}";
}
