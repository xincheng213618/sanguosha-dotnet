#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouDongZhaoCapability(Func<RuntimeEntries.CharacterDongZhaoMouOlEntries> getCharacterDongZhaoMouOlRuntime) : IMouDongZhaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => operation switch
    {
        SkillProgramEffectOp.ResolveMouDongZhaoShunji => getCharacterDongZhaoMouOlRuntime().BeginMouShunji(frame),
        SkillProgramEffectOp.ResolveMouDongZhaoYishi => getCharacterDongZhaoMouOlRuntime().BeginMouYishi(frame),
        _ => getCharacterDongZhaoMouOlRuntime().BeginMouYishiLoss(frame)};
}
