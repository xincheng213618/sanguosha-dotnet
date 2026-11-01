using System.Text.Json.Serialization;
namespace CardGame.Core;
internal sealed class ResolveZhijieActualTurnEndDescriptor():CuiYanDescriptor(SkillProgramEffectOp.ResolveZhijieActualTurnEnd,SkillProgramTriggerWindow.AfterTurnEnded) {
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeActionAppearance);
    public override ProgramDependencyKinds TracksActualTurnCursorReturn => ProgramDependencyKinds.Trigger; }
