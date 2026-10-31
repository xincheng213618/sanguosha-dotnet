#nullable enable
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class GuanduGeneralHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineGuanduGeneralsEntries> getCoreGameEngineGuanduGeneralsRuntime) : IGuanduGeneralHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCoreGameEngineGuanduGeneralsRuntime().BeginGuanduGeneral(frame, operation);
}
