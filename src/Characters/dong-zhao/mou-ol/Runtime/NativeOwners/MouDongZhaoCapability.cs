#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class MouDongZhaoCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDongZhaoMouOlEntries> getCharacterDongZhaoMouOlRuntime) : IMouDongZhaoHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => operation switch
    {
        SkillProgramEffectOp.ResolveMouDongZhaoShunji => getCharacterDongZhaoMouOlRuntime().BeginMouShunji(frame),
        SkillProgramEffectOp.ResolveMouDongZhaoYishi => getCharacterDongZhaoMouOlRuntime().BeginMouYishi(frame),
        _ => getCharacterDongZhaoMouOlRuntime().BeginMouYishiLoss(frame)};
}
