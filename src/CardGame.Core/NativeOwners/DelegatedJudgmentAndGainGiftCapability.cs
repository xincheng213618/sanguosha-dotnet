#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DelegatedJudgmentAndGainGiftCapability(Func<RuntimeEntries.CoreGameEngineGainGiftProgramsEntries> getCoreGameEngineGainGiftProgramsRuntime) : IDelegatedJudgmentAndGainGiftHost
{
    public SkillProgramStepOutcome ExecuteGainGift(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineGainGiftProgramsRuntime().ExecuteGainGift(frame, effect);
}
