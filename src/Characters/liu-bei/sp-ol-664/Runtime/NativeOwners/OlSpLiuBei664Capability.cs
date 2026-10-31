#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OlSpLiuBei664Capability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLiuBeiSpOl664Runtime>> getCharacterLiuBeiSpOl664Runtime) : IOlSpLiuBei664Host
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffect effect) => effect.Op switch
    {
        SkillProgramEffectOp.UseOlSpLiuBeiZhujiu => getCharacterLiuBeiSpOl664Runtime().Value.BeginOlSpZhujiu(frame),
        SkillProgramEffectOp.GrantOlSpLiuBeiXudai => getCharacterLiuBeiSpOl664Runtime().Value.GrantOlSpXudai(frame),
        SkillProgramEffectOp.ResolveOlSpLiuBeiJinglei when effect.StateId == "pay" => getCharacterLiuBeiSpOl664Runtime().Value.PayOlSpJinglei(frame),
        _ => getCharacterLiuBeiSpOl664Runtime().Value.BeginOlSpJingleiAdjustment(frame)};
}
