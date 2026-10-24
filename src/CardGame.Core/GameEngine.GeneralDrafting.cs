namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool UsesFamilyGeneralDraft => _modeDefinition.AiGeneralSwapExcludedIds is not null ||
        _modeDefinition.GeneralSelectionWeights is not null || _modeDefinition.GeneralSelectionPoolIds is not null;

}
