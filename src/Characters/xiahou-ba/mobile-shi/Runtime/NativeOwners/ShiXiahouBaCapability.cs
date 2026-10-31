#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShiXiahouBaCapability(Func<RuntimeEntries.CharacterXiahouBaMobileShiEntries> getCharacterXiahouBaMobileShiRuntime) : IShiXiahouBaProgramHost
{
    public SkillProgramStepOutcome ResolveLibbing(ProgramSkillFrame frame) => getCharacterXiahouBaMobileShiRuntime().BeginShiXb(frame, SkillProgramEffectOp.ResolveShiXbLibbing);
    public SkillProgramStepOutcome ResolvePoxi(ProgramSkillFrame frame) => getCharacterXiahouBaMobileShiRuntime().BeginShiXb(frame, SkillProgramEffectOp.ResolveShiXbPoxi);
}
