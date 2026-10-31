#nullable enable
using System.Text.Json.Serialization;

namespace CardGame.Core.NativeOwners;
internal sealed class DragonBoatGeneralHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDragonBoatGeneralsEntries> getCoreGameEngineDragonBoatGeneralsRuntime) : IDragonBoatGeneralHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation, int targetSeat) => getCoreGameEngineDragonBoatGeneralsRuntime().BeginDragonBoatGeneral(frame, operation, targetSeat);
}
