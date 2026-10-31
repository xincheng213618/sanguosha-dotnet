#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryMuShunHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterMuShunAnniversaryEntries> getCharacterMuShunAnniversaryRuntime) : IAnniversaryMuShunHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => operation == SkillProgramEffectOp.ResolveAnniversaryMuShunJinjian ? getCharacterMuShunAnniversaryRuntime().BeginMuShunJinjian(frame) : getCharacterMuShunAnniversaryRuntime().ResolveMuShunShizhao(frame);
}
