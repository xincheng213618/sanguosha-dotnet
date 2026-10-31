namespace CardGame.Core;
public sealed partial class GameEngine
{
    private ref global::CardGame.Core.GuanduBattleConfiguredEvent? _guanduInitialConfiguration => ref _guanduActivities.ReferenceToGuanduInitialConfiguration;
    private ref long _guanduQualificationStamp => ref _guanduActivities.ReferenceToGuanduQualificationStamp;
    private ref (long Underlying, int LivingMask, int FactionHash) _guanduQualificationDependencies => ref _guanduActivities.ReferenceToGuanduQualificationDependencies;
}
