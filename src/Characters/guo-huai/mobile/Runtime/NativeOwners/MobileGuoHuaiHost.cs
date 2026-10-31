#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileGuoHuaiHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuoHuaiMobileEntries> getCharacterGuoHuaiMobileRuntime) : IMobileGuoHuaiHost
{
    public void Prepare(ProgramSkillFrame supplied)
    {
        var frame = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(supplied.Id);
        if (frame.WindowContext?.Window != SkillProgramTriggerWindow.PlayEnding || !getCharacterGuoHuaiMobileRuntime().CanRunMobileGuoHuaiJingce(frame.OwnerSeat))
            throw new InvalidOperationException("Jingce requires enough actual uses in its owner's current Play phase.");
    }
}
