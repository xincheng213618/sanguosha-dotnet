#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryMuShunHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterMuShunAnniversaryRuntime>> getCharacterMuShunAnniversaryRuntime) : IAnniversaryMuShunHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => operation == SkillProgramEffectOp.ResolveAnniversaryMuShunJinjian ? getCharacterMuShunAnniversaryRuntime().Value.BeginMuShunJinjian(frame) : getCharacterMuShunAnniversaryRuntime().Value.ResolveMuShunShizhao(frame);
}
