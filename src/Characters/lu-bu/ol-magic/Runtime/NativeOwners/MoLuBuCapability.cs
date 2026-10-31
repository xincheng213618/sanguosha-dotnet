#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MoLuBuCapability(Func<RuntimeEntries.CharacterLuBuOlMagicEntries> getCharacterLuBuOlMagicRuntime) : IMoLuBuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.InitializeMoLuBuQiAndOpeningPlay => getCharacterLuBuOlMagicRuntime().InitializeMoLuBu(f),
        SkillProgramEffectOp.ResolveMoLuBuDuoqi => getCharacterLuBuOlMagicRuntime().BeginMoDuoqi(f),
        SkillProgramEffectOp.EnterMoLuBuKuangmo => getCharacterLuBuOlMagicRuntime().BeginMoKuangmo(f),
        _ => getCharacterLuBuOlMagicRuntime().BeginMoKuangmoDeath(f)};
}
