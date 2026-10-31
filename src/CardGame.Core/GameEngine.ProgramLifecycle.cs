namespace CardGame.Core;
/// <summary>Generic lifecycle-program host. It contains no character or skill ids.</summary>
public sealed partial class GameEngine
{
    private ProgramInteractionRouter? _programInteractions;
    private ProgramInteractionRouter ProgramInteractions => _programInteractions ??= new(ProgramInteractionCatalog.Default, BindRuntimeHandler, this.ProgramLifecycleEntries.ResolveProgramActivationChoice, _modeRuntime.PromptAnswers);
    private ref global::CardGame.Core.ProgramPhaseSchedule? _programPhaseSchedule => ref phaseScheduleStorage.Current;
}
