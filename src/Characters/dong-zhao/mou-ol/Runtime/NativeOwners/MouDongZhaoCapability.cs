#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouDongZhaoCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterDongZhaoMouOlRuntime>> getCharacterDongZhaoMouOlRuntime) : IMouDongZhaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => operation switch
    {
        SkillProgramEffectOp.ResolveMouDongZhaoShunji => getCharacterDongZhaoMouOlRuntime().Value.BeginMouShunji(frame),
        SkillProgramEffectOp.ResolveMouDongZhaoYishi => getCharacterDongZhaoMouOlRuntime().Value.BeginMouYishi(frame),
        _ => getCharacterDongZhaoMouOlRuntime().Value.BeginMouYishiLoss(frame)};
}
