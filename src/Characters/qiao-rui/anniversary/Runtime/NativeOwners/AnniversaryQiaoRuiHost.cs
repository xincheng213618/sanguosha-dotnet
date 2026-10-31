#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryQiaoRuiHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterQiaoRuiAnniversaryRuntime>> getCharacterQiaoRuiAnniversaryRuntime) : IAnniversaryQiaoRuiHost
{
    public SkillProgramStepOutcome Begin(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterQiaoRuiAnniversaryRuntime().Value.BeginQiaoRui(f, op);
}
