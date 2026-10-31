#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HeYanHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterHeYanAnniversaryRuntime>> getCharacterHeYanAnniversaryRuntime) : IAnniversaryHeYanHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHeYanAnniversaryRuntime().Value.BeginHeYan(f, op);
}
