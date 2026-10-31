#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess : IProgramRuntimeOperations
{
    ref long _guanduQualificationStamp { get; }

    ref (long Underlying, int LivingMask, int FactionHash) _guanduQualificationDependencies { get; }

    ref GuanduBattleConfiguredEvent? _guanduInitialConfiguration { get; }
}
