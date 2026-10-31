#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class WushuangSkillsBCapability(Func<RuntimeEntries.CoreGameEngineWushuangSkillsBEntries> getCoreGameEngineWushuangSkillsBRuntime) : IWushuangSkillsBHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCoreGameEngineWushuangSkillsBRuntime().BeginWushuangB(f, op);
}
