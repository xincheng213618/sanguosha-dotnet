
namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_ForeignSelectedCardSlash
{
    internal static CardConversionSource SelectedForeignSlashSource(ProgramSkillFrame frame) =>
        new(frame.SkillId, frame.ActivationId!, frame.OwnerSeat, frame.SkillInstanceId);
}
