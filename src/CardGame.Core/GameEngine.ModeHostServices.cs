namespace CardGame.Core;

public sealed partial class GameEngine
{
    private ModeHostRegistry CreateModeHostServices() => new ModeHostRegistry()
        .Register<IWushuangModeHost>(() => this.ComposeNativeWushuangModeHost())
        .Register<IModeSetupRandom>(() => this.ComposeNativeModeSetupRandom())
        .Register<IModeEncounterHost>(CreateModeEncounterHost)
        .Register<IHellEncounterHost>(CreateHellEncounterHost)
        .Register<INianChallengeHost>(CreateNianChallengeHost)
        .Register<IStandardBattleHost>(CreateStandardBattleHost)
        .Register<IModeBattleRulesHost>(CreateActivityBattleRulesHost)
        .Register<IWenheRuleFlowHost>(CreateWenheRuleFlowHost)
        .Register<IGuanduEventFlowHost>(CreateGuanduEventFlowHost)
        .Register<IModeNationalRevealHost>(CreateModeNationalRevealHost)
        .Register<IModeSetupEquipmentHost>(CreateModeSetupEquipmentHost);
}
