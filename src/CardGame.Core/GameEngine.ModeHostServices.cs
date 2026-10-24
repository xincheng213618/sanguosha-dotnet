namespace CardGame.Core;

public sealed partial class GameEngine
{
    private ModeHostRegistry CreateModeHostServices() => new ModeHostRegistry()
        .Register<IWushuangModeHost>(() => new WushuangModeHost(this))
        .Register<IModeSetupRandom>(() => new ModeSetupRandom(this))
        .Register<IModeEncounterHost>(CreateModeEncounterHost)
        .Register<IHellEncounterHost>(CreateHellEncounterHost)
        .Register<INianChallengeHost>(CreateNianChallengeHost)
        .Register<IStandardBattleHost>(CreateStandardBattleHost)
        .Register<IModeBattleRulesHost>(CreateActivityBattleRulesHost)
        .Register<IWenheRuleFlowHost>(CreateWenheRuleFlowHost)
        .Register<IGuanduEventFlowHost>(CreateGuanduEventFlowHost)
        .Register<IModeNationalRevealHost>(CreateModeNationalRevealHost)
        .Register<IModeSetupEquipmentHost>(CreateModeSetupEquipmentHost);

    private sealed class ModeSetupRandom(GameEngine engine) : IModeSetupRandom
    {
        public int Next(int exclusiveMax) => engine._random.Next(exclusiveMax);
        public void Shuffle<T>(IList<T> values) => engine._random.Shuffle(values);
    }
}
