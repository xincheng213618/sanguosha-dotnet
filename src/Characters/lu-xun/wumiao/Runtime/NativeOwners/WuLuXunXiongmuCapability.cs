#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class WuLuXunXiongmuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuXunWumiaoEntries> getCharacterLuXunWumiaoRuntime) : IWuLuXunXiongmuHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveWuLuXunXiongmu ? getCharacterLuXunWumiaoRuntime().BeginWuXiongmu(f) : getCharacterLuXunWumiaoRuntime().ResolveWuXiongmuFirstDamage(f);
}
