#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryFengXiHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterFengXiAnniversaryRuntime>> getCharacterFengXiAnniversaryRuntime) : IAnniversaryFengXiHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterFengXiAnniversaryRuntime().Value.BeginFengXi(f, op);
}
