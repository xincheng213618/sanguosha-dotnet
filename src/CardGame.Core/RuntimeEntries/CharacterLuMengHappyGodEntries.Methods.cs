#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuMengHappyGodEntries
{
    internal void BindHappyGongxin(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BindHappyGongxin(supplied);
    internal bool HappyGongxinEligible(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().HappyGongxinEligible(c, t, context);
    internal bool HasExactHappyGongxinParticipantSelection(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HasExactHappyGongxinParticipantSelection(f);
    internal bool IsExactHappyGongxinSelfReveal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsExactHappyGongxinSelfReveal(f);
    internal bool TryHappyGongxinCounterpart(int owner, string binding, global::CardGame.Core.ProgramSkillWindowContext context, scoped out global::CardGame.Core.CardUseFrame use, scoped out int target, scoped out int other) => _resolve().TryHappyGongxinCounterpart(owner, binding, context, out use, out target, out other);
}
