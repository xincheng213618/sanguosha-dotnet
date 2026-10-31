#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouZhangXiuOlCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangXiuMouOlRuntime>> getCharacterZhangXiuMouOlRuntime) : IMouZhangXiuOlHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveMouZhangXiuQiulie or SkillProgramEffectOp.UseMouZhangXiuQiulieSlash => getCharacterZhangXiuMouOlRuntime().Value.BeginMouZhangXiuOlQiulie(f, op),
        SkillProgramEffectOp.DefendMouZhangXiuQiulieSlash => getCharacterZhangXiuMouOlRuntime().Value.BeginMouZhangXiuOlDefense(f),
        _ => getCharacterZhangXiuMouOlRuntime().Value.BeginMouZhangXiuOlZhuijiao(f, op)};
}
