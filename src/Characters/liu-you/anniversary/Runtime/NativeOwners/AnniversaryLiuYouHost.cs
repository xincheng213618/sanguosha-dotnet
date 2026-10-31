#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryLiuYouHost(Func<RuntimeEntries.CharacterLiuYouAnniversaryEntries> getCharacterLiuYouAnniversaryRuntime) : IAnniversaryLiuYouHost
{
    public void Execute(ProgramSkillFrame f, SkillProgramEffectOp op)
    {
        if (op == SkillProgramEffectOp.PrepareAnniversaryLiuYouKannan)
            getCharacterLiuYouAnniversaryRuntime().PrepareLiuYou(f);
        else
            getCharacterLiuYouAnniversaryRuntime().ResolveLiuYou(f);
    }
}
