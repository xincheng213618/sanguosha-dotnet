namespace CardGame.Core;
public sealed partial class GameEngine
{
    // Projections and their cache belong to this match. Registered source programs keep their content identity.
    private ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.SkillProgram, global::CardGame.Core.SkillProgram> _activityOrdinaryPrograms => ref _activityOrdinaryTricks.ReferenceToActivityOrdinaryPrograms;

    /// <summary>Returns immutable rule metadata for a published conversion source in this match's mode.</summary>
    public SkillProgramViewAs? GetCardConversionDefinition(CardConversionSource source) => GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreGameEngineActivityOrdinaryTricksRuntime>().GetCardConversionDefinition(source);
}
