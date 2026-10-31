namespace CardGame.Core;

public sealed partial class GameEngine
{
    private ProgramTargetResolverRegistry? _programTargetResolvers;
    private ProgramTargetResolverRegistry ProgramTargetResolvers =>
        _programTargetResolvers ??= new(global::CardGame.Core.EngineOwners.CoreGameEngineProgramTargetResolversDefinitions.ProgramTargetResolverMetadata.Value, ProgramSelectionQueries);

}
