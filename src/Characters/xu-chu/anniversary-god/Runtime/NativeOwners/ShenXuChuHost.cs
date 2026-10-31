#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShenXuChuHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterXuChuAnniversaryGodRuntime>> getCharacterXuChuAnniversaryGodRuntime) : IShenXuChuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterXuChuAnniversaryGodRuntime().Value.BeginShenXuChu(f, op);
}
