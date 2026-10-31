#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MobileGuYongHost(Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreGameEngineProgramCategoryChallengesEntries> getCoreGameEngineProgramCategoryChallengesRuntime, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime) : IMobileGuYongHost
{
    public SkillProgramStepOutcome CompleteMixedHand(ProgramSkillFrame supplied, string binding)
    {
        var frame = getCoreGameEngineProgramLifecycleRuntime().GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != "mobile:bingyi" || frame.TriggerId != "ending-reveal-and-share" || frame.InstructionIndex != 3 || binding != "shown-hand" || getCoreGameEngineProgramLifecycleRuntime().GetProgramCardSet(frame, binding).Visibility != SkillProgramCardSetVisibility.Public)
            throw new InvalidOperationException("Bingyi requires its original already-revealed whole-hand binding.");
        if (getCoreGameEngineProgramCategoryChallengesRuntime().AreProgramBoundCardsSameColor(frame, binding))
            return SkillProgramStepOutcome.Continue;
        getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(frame, completed: true);
        return SkillProgramStepOutcome.AwaitChild;
    }
}
