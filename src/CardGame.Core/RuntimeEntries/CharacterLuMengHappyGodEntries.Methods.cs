#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuMengHappyGodEntries
{
    internal void BindHappyGongxin(ProgramSkillFrame supplied) => _resolve().BindHappyGongxin(supplied);
    internal bool HappyGongxinEligible(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().HappyGongxinEligible(c, t, context);
    internal bool HasExactHappyGongxinParticipantSelection(ProgramSkillFrame f) => _resolve().HasExactHappyGongxinParticipantSelection(f);
    internal bool IsExactHappyGongxinSelfReveal(ProgramSkillFrame f) => _resolve().IsExactHappyGongxinSelfReveal(f);
    internal bool TryHappyGongxinCounterpart(int owner, string binding, ProgramSkillWindowContext context, scoped out CardUseFrame use, scoped out int target, scoped out int other) => _resolve().TryHappyGongxinCounterpart(owner, binding, context, out use, out target, out other);
}
