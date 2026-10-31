namespace CardGame.Core;

public sealed partial class GameEngine
{
    private ProgramSelectionValidationRegistry? _programSelectionValidators;
    private ProgramSelectionValidationRegistry ProgramSelectionValidators =>
        _programSelectionValidators ??= new(global::CardGame.Core.EngineOwners.CoreGameEngineProgramSelectionValidationDefinitions.ProgramSelectionValidationMetadata.Value, ProgramSelectionQueries);
}
