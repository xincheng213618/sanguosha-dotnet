#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class ShiXiahouBaCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXiahouBaMobileShiEntries> getCharacterXiahouBaMobileShiRuntime) : IShiXiahouBaProgramHost
{
    public SkillProgramStepOutcome ResolveLibbing(ProgramSkillFrame frame) => getCharacterXiahouBaMobileShiRuntime().BeginShiXb(frame, SkillProgramEffectOp.ResolveShiXbLibbing);
    public SkillProgramStepOutcome ResolvePoxi(ProgramSkillFrame frame) => getCharacterXiahouBaMobileShiRuntime().BeginShiXb(frame, SkillProgramEffectOp.ResolveShiXbPoxi);
}
