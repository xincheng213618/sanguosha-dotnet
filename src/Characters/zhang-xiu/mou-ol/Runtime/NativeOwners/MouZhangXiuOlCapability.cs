#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouZhangXiuOlCapability(Func<RuntimeEntries.CharacterZhangXiuMouOlEntries> getCharacterZhangXiuMouOlRuntime) : IMouZhangXiuOlHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveMouZhangXiuQiulie or SkillProgramEffectOp.UseMouZhangXiuQiulieSlash => getCharacterZhangXiuMouOlRuntime().BeginMouZhangXiuOlQiulie(f, op),
        SkillProgramEffectOp.DefendMouZhangXiuQiulieSlash => getCharacterZhangXiuMouOlRuntime().BeginMouZhangXiuOlDefense(f),
        _ => getCharacterZhangXiuMouOlRuntime().BeginMouZhangXiuOlZhuijiao(f, op)};
}
