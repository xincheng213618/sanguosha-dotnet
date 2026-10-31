#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5XuchuEntries
{
    internal bool CanOfferShanheS5Luoyi(ProgramTriggerLegalityContext c) => _resolve().CanOfferShanheS5Luoyi(c);
    internal void ResolveShanheS5Luoyi(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().ResolveShanheS5Luoyi(supplied, effect);
    internal ProgramLifecycleTriggerWindowFrame ShanheS5LuoyiDrawPlan(ProgramSkillFrame f) => _resolve().ShanheS5LuoyiDrawPlan(f);
    internal ShanheS5LuoyiFrozenEvent ShanheS5LuoyiFrozen(ProgramSkillFrame f, string bind) => _resolve().ShanheS5LuoyiFrozen(f, bind);
}
