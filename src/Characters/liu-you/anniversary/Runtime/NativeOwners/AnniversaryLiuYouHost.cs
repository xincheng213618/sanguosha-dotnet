#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryLiuYouHost(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLiuYouAnniversaryRuntime>> getCharacterLiuYouAnniversaryRuntime) : IAnniversaryLiuYouHost
{
    public void Execute(ProgramSkillFrame f, SkillProgramEffectOp op)
    {
        if (op == SkillProgramEffectOp.PrepareAnniversaryLiuYouKannan)
            getCharacterLiuYouAnniversaryRuntime().Value.PrepareLiuYou(f);
        else
            getCharacterLiuYouAnniversaryRuntime().Value.ResolveLiuYou(f);
    }
}
