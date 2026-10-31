#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class LiuZhangCapability(Func<RuntimeEntries.CharacterLiuZhangOlEntries> getCharacterLiuZhangOlRuntime) : ILiuZhangHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.ResolveZonghuRequiredUseRequest ? getCharacterLiuZhangOlRuntime().ResolveZonghu(f) : getCharacterLiuZhangOlRuntime().ResolveFengwei(f, op);
}
