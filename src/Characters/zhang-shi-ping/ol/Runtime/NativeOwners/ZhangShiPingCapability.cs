#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhangShiPingCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangShiPingOlRuntime>> getCharacterZhangShiPingOlRuntime) : IZhangShiPingProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ArmHongjiNextPhase => getCharacterZhangShiPingOlRuntime().Value.BeginHongjiPromise(f),
        SkillProgramEffectOp.ResolveHongjiGrantedPhase => getCharacterZhangShiPingOlRuntime().Value.BeginHongjiExtraPhase(f),
        _ => getCharacterZhangShiPingOlRuntime().Value.BeginXingGu(f, op)};
}
