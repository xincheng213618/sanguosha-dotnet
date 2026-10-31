#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeProgramExecution(Func<RuntimeEntries.CharacterHuaTuoAnniversaryGodEntries> getCharacterHuaTuoAnniversaryGodRuntime, GetNativeTryResumeTarget getTryResumeTarget, ProgramSkillHostFactory createProgramSkillHost) : INativeProgramExecution
{
    public bool TryResume(long frameId) => getCharacterHuaTuoAnniversaryGodRuntime().TryBeginProgramSkillInvocation(frameId) || getTryResumeTarget()(frameId);
    public void Execute(long frameId)
    {
        if (getCharacterHuaTuoAnniversaryGodRuntime().TryBeginProgramSkillInvocation(frameId))
            return;
        var host = createProgramSkillHost();
        new SkillProgramExecutor().Run(frameId, host, host);
    }
}
