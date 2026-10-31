#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class GuanduGeneralHost(Func<RuntimeEntries.CoreGameEngineGuanduGeneralsEntries> getCoreGameEngineGuanduGeneralsRuntime) : IGuanduGeneralHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCoreGameEngineGuanduGeneralsRuntime().BeginGuanduGeneral(frame, operation);
}
