#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryHaoMengHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterHaoMengAnniversaryRuntime>> getCharacterHaoMengAnniversaryRuntime) : IAnniversaryHaoMengHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHaoMengAnniversaryRuntime().Value.BeginHaoMeng(f, op);
}
