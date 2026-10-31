#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryCheZhouHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCheZhouAnniversaryRuntime>> getCharacterCheZhouAnniversaryRuntime) : IAnniversaryCheZhouHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCharacterCheZhouAnniversaryRuntime().Value.BeginAnniversaryCheZhouPigua(f);
}
