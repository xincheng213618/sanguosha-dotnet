
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_cao_zhi_le_LeCaoZhiWenlan
{
    internal static WenlanSource WenlanFrameSource(ProgramSkillFrame f) =>
        new(new(f.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash);

    internal static CardKind[] WenlanNames(WenlanActionAcceptedEvent entry) => entry.Capture.FuEntity is not { } fu || entry.Capture.MaterialCount != 1
        ? [] : (fu.AliasKind is { } alias ? new[] { fu.PrintedKind, alias } : [fu.PrintedKind]).Distinct().Order().ToArray();
}
