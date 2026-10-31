#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref long _guanduQualificationStamp { get; }

    ref (long Underlying, int LivingMask, int FactionHash) _guanduQualificationDependencies { get; }

    ref global::CardGame.Core.GuanduBattleConfiguredEvent? _guanduInitialConfiguration { get; }
}
