#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5XuchuEntries
{
    internal bool CanOfferShanheS5Luoyi(global::CardGame.Core.ProgramTriggerLegalityContext c) => _resolve().CanOfferShanheS5Luoyi(c);
    internal void ResolveShanheS5Luoyi(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ResolveShanheS5Luoyi(supplied, effect);
    internal global::CardGame.Core.ProgramLifecycleTriggerWindowFrame ShanheS5LuoyiDrawPlan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShanheS5LuoyiDrawPlan(f);
    internal global::CardGame.Core.ShanheS5LuoyiFrozenEvent ShanheS5LuoyiFrozen(global::CardGame.Core.ProgramSkillFrame f, string bind) => _resolve().ShanheS5LuoyiFrozen(f, bind);
}
