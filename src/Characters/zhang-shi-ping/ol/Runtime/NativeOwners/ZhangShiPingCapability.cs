#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ZhangShiPingCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangShiPingOlEntries> getCharacterZhangShiPingOlRuntime) : IZhangShiPingProgramHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ArmHongjiNextPhase => getCharacterZhangShiPingOlRuntime().BeginHongjiPromise(f),
        SkillProgramEffectOp.ResolveHongjiGrantedPhase => getCharacterZhangShiPingOlRuntime().BeginHongjiExtraPhase(f),
        _ => getCharacterZhangShiPingOlRuntime().BeginXingGu(f, op)};
}
