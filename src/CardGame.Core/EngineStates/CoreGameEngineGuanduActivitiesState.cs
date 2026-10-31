#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineGuanduActivitiesState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess
{
    private global::CardGame.Core.GuanduBattleConfiguredEvent? _guanduInitialConfiguration = default !;
    internal ref global::CardGame.Core.GuanduBattleConfiguredEvent? ReferenceToGuanduInitialConfiguration => ref _guanduInitialConfiguration;

    private (long Underlying, int LivingMask, int FactionHash) _guanduQualificationDependencies = default !;
    internal ref (long Underlying, int LivingMask, int FactionHash) ReferenceToGuanduQualificationDependencies => ref _guanduQualificationDependencies;

    private long _guanduQualificationStamp = default !;
    internal ref long ReferenceToGuanduQualificationStamp => ref _guanduQualificationStamp;

    ref long global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess._guanduQualificationStamp => ref _guanduQualificationStamp;

    ref (long Underlying, int LivingMask, int FactionHash) global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess._guanduQualificationDependencies => ref _guanduQualificationDependencies;

    ref global::CardGame.Core.GuanduBattleConfiguredEvent? global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess._guanduInitialConfiguration => ref _guanduInitialConfiguration;
}
