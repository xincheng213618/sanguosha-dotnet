#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WuLuXunXiongmuCapability(Func<RuntimeEntries.CharacterLuXunWumiaoEntries> getCharacterLuXunWumiaoRuntime) : IWuLuXunXiongmuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveWuLuXunXiongmu ? getCharacterLuXunWumiaoRuntime().BeginWuXiongmu(f) : getCharacterLuXunWumiaoRuntime().ResolveWuXiongmuFirstDamage(f);
}
