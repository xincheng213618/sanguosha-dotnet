#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class WushuangSkillsBCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangSkillsBEntries> getCoreGameEngineWushuangSkillsBRuntime) : IWushuangSkillsBHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCoreGameEngineWushuangSkillsBRuntime().BeginWushuangB(f, op);
}
