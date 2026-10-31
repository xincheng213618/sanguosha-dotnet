#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class DelegatedJudgmentAndGainGiftCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineGainGiftProgramsEntries> getCoreGameEngineGainGiftProgramsRuntime) : IDelegatedJudgmentAndGainGiftHost
{
    public SkillProgramStepOutcome ExecuteGainGift(ProgramSkillFrame frame, SkillProgramEffect effect) => getCoreGameEngineGainGiftProgramsRuntime().ExecuteGainGift(frame, effect);
}
