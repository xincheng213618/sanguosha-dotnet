#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileGuoHuaiHost(Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CharacterGuoHuaiMobileEntries> getCharacterGuoHuaiMobileRuntime) : IMobileGuoHuaiHost
{
    public void Prepare(ProgramSkillFrame supplied)
    {
        var frame = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(supplied.Id);
        if (frame.WindowContext?.Window != SkillProgramTriggerWindow.PlayEnding || !getCharacterGuoHuaiMobileRuntime().CanRunMobileGuoHuaiJingce(frame.OwnerSeat))
            throw new InvalidOperationException("Jingce requires enough actual uses in its owner's current Play phase.");
    }
}
