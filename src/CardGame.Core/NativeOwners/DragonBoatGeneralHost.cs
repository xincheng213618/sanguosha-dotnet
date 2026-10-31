#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DragonBoatGeneralHost(Func<RuntimeEntries.CoreGameEngineDragonBoatGeneralsEntries> getCoreGameEngineDragonBoatGeneralsRuntime) : IDragonBoatGeneralHost
{
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation, int targetSeat) => getCoreGameEngineDragonBoatGeneralsRuntime().BeginDragonBoatGeneral(frame, operation, targetSeat);
}
