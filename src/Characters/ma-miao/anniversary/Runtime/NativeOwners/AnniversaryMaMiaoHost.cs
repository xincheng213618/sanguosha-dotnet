#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryMaMiaoHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterMaMiaoAnniversaryRuntime>> getCharacterMaMiaoAnniversaryRuntime) : IAnniversaryMaMiaoHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterMaMiaoAnniversaryRuntime().Value.BeginMaMiao(f, op);
}
