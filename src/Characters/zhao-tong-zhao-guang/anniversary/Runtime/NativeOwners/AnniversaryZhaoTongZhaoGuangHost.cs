#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryZhaoTongZhaoGuangHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhaoTongZhaoGuangAnniversaryRuntime>> getCharacterZhaoTongZhaoGuangAnniversaryRuntime) : IAnniversaryZhaoTongZhaoGuangHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterZhaoTongZhaoGuangAnniversaryRuntime().Value.BeginAnniversaryZhaoTongZhaoGuang(f, op);
}
