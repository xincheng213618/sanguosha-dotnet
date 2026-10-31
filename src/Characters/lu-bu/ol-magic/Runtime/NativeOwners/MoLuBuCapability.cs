#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MoLuBuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuBuOlMagicEntries> getCharacterLuBuOlMagicRuntime) : IMoLuBuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.InitializeMoLuBuQiAndOpeningPlay => getCharacterLuBuOlMagicRuntime().InitializeMoLuBu(f),
        SkillProgramEffectOp.ResolveMoLuBuDuoqi => getCharacterLuBuOlMagicRuntime().BeginMoDuoqi(f),
        SkillProgramEffectOp.EnterMoLuBuKuangmo => getCharacterLuBuOlMagicRuntime().BeginMoKuangmo(f),
        _ => getCharacterLuBuOlMagicRuntime().BeginMoKuangmoDeath(f)};
}
