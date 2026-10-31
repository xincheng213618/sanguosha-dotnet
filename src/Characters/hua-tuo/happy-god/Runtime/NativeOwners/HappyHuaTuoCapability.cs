#nullable enable
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class HappyHuaTuoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuaTuoHappyGodEntries> getCharacterHuaTuoHappyGodRuntime) : IHappyHuaTuoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterHuaTuoHappyGodRuntime().ResolveHappyHuaTuo(f, op);
}
