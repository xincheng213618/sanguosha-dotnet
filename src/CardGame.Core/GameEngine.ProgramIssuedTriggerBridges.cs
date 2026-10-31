namespace CardGame.Core;

public sealed partial class GameEngine
{
    private ProgramIssuedTriggerRuntime? _programIssuedTriggers;
    private ProgramIssuedTriggerRuntime ProgramIssuedTriggers => _programIssuedTriggers ??=
        new(ProgramIssuedTriggerCatalog.Default, ResolveRuntimeHandlerTarget);

    private ProgramTriggerProofRuntime? _programTriggerProofs;
    private ProgramTriggerProofRuntime ProgramTriggerProofs => _programTriggerProofs ??=
        new(ProgramTriggerProofCatalog.Default, ResolveRuntimeHandlerTarget);
}
