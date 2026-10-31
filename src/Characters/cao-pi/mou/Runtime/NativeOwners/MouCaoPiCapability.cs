#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouCaoPiCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoPiMouEntries> getCharacterCaoPiMouRuntime) : IMouCaoPiHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterCaoPiMouRuntime().BeginMouCaoPi(f, op);
}
