#nullable enable
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CardGame.Core.NativeOwners;
internal sealed class LiuZhangCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiuZhangOlEntries> getCharacterLiuZhangOlRuntime) : ILiuZhangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveZonghuRequiredUseRequest ? getCharacterLiuZhangOlRuntime().ResolveZonghu(f) : getCharacterLiuZhangOlRuntime().ResolveFengwei(f, op);
}
