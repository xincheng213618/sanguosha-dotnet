#nullable enable

namespace CardGame.Core.EngineOwners;
internal static class CoreGameEngineProgramSelectionValidationDefinitions
{
internal static readonly Lazy<ProgramSelectionValidationCatalog> ProgramSelectionValidationMetadata = new(() => new(typeof(ProgramSelectionValidationCatalog).Assembly.GetTypes()), true);
}
