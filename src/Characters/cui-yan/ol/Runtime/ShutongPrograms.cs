using System.Text.Json.Serialization;
namespace CardGame.Core;
internal sealed class ResolveShutongFinalizedUseDescriptor():CuiYanDescriptor(SkillProgramEffectOp.ResolveShutongFinalizedUse,SkillProgramTriggerWindow.CardUseTargetsFinalized) {
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeActionAppearance | ProgramTrackedFact.ActualCardUseHistory | ProgramTrackedFact.ActualTurnReturn | ProgramTrackedFact.NativeLegacyActualUseCompletion);
    public override ProgramDependencyKinds TracksActualTurnCursorReturn => ProgramDependencyKinds.Trigger; }
