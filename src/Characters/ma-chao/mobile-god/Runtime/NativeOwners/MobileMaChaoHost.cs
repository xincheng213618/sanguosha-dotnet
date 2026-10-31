#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MobileMaChaoHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterMaChaoMobileGodEntries> getCharacterMaChaoMobileGodRuntime) : IMobileShenMaChaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterMaChaoMobileGodRuntime().BeginMobileMaChao(f, op);
}
