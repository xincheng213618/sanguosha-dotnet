#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryLiuYouHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiuYouAnniversaryEntries> getCharacterLiuYouAnniversaryRuntime) : IAnniversaryLiuYouHost
{
    public void Execute(ProgramSkillFrame f, SkillProgramEffectOp op)
    {
        if (op == SkillProgramEffectOp.PrepareAnniversaryLiuYouKannan)
            getCharacterLiuYouAnniversaryRuntime().PrepareLiuYou(f);
        else
            getCharacterLiuYouAnniversaryRuntime().ResolveLiuYou(f);
    }
}
