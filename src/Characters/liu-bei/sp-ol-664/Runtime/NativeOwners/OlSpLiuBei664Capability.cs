#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OlSpLiuBei664Capability(Func<RuntimeEntries.CharacterLiuBeiSpOl664Entries> getCharacterLiuBeiSpOl664Runtime) : IOlSpLiuBei664Host
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffect effect) => effect.Op switch
    {
        SkillProgramEffectOp.UseOlSpLiuBeiZhujiu => getCharacterLiuBeiSpOl664Runtime().BeginOlSpZhujiu(frame),
        SkillProgramEffectOp.GrantOlSpLiuBeiXudai => getCharacterLiuBeiSpOl664Runtime().GrantOlSpXudai(frame),
        SkillProgramEffectOp.ResolveOlSpLiuBeiJinglei when effect.StateId == "pay" => getCharacterLiuBeiSpOl664Runtime().PayOlSpJinglei(frame),
        _ => getCharacterLiuBeiSpOl664Runtime().BeginOlSpJingleiAdjustment(frame)};
}
