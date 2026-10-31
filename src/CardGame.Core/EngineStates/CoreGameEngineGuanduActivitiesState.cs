#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineGuanduActivitiesState : Contracts.ICoreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess
{
    private GuanduBattleConfiguredEvent? _guanduInitialConfiguration = default !;
    internal ref GuanduBattleConfiguredEvent? ReferenceToGuanduInitialConfiguration => ref _guanduInitialConfiguration;

    private (long Underlying, int LivingMask, int FactionHash) _guanduQualificationDependencies = default !;
    internal ref (long Underlying, int LivingMask, int FactionHash) ReferenceToGuanduQualificationDependencies => ref _guanduQualificationDependencies;

    private long _guanduQualificationStamp = default !;
    internal ref long ReferenceToGuanduQualificationStamp => ref _guanduQualificationStamp;

    ref long Contracts.ICoreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess._guanduQualificationStamp => ref _guanduQualificationStamp;

    ref (long Underlying, int LivingMask, int FactionHash) Contracts.ICoreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess._guanduQualificationDependencies => ref _guanduQualificationDependencies;

    ref GuanduBattleConfiguredEvent? Contracts.ICoreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess._guanduInitialConfiguration => ref _guanduInitialConfiguration;
}
