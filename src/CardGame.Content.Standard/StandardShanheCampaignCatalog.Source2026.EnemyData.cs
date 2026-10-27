using System.Collections.ObjectModel;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    // Exact base quantities and cumulative att_* rows from the official hd_roguelike General table.
    // Enemy HP/MaxHp growth and stage entry use the shared, explicitly local source policy.
    private sealed record Source2026EnemyData(int SourceGeneralId, int Hp, int MaxHp, int Draw, int StartingCards,
        int ExtraSlash, int Armor, int ArmorRecovery, int ArmorCap, int ExtraDamage, bool Hidden,
        IReadOnlyList<ShanheSourceEnemyTierAttributes> TierAttributes);

    private static readonly IReadOnlyDictionary<int, Source2026EnemyData[]> Source2026EnemyDataByGroup =
        new ReadOnlyDictionary<int, Source2026EnemyData[]>(new Dictionary<int, Source2026EnemyData[]>
        {
            [2473] =
            [
                new(4861, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, []),
            ],
            [2480] =
            [
                new(4871, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, []),
            ],
            [2467] =
            [
                new(4861, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, []),
            ],
            [2474] =
            [
                new(4861, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, []),
            ],
            [2487] =
            [
                new(4861, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, []),
            ],
            [2632] =
            [
                new(602, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2633] =
            [
                new(4871, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2634] =
            [
                new(427, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(4871, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2635] =
            [
                new(587, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2636] =
            [
                new(418, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2637] =
            [
                new(424, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, []),
            ],
            [2638] =
            [
                new(2081, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2639] =
            [
                new(476, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, []),
            ],
            [2640] =
            [
                new(538, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2641] =
            [
                new(417, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2642] =
            [
                new(303, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(346, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2643] =
            [
                new(73, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(487, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2644] =
            [
                new(319, 5, 5, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2645] =
            [
                new(319, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(1, 0, 0, 0, 5, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2646] =
            [
                new(314, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, []),
            ],
            [2647] =
            [
                new(446, 5, 5, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(1, 0, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2648] =
            [
                new(160, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2649] =
            [
                new(205, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(1, 0, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2650] =
            [
                new(229, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(501, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2651] =
            [
                new(313, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(453, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2652] =
            [
                new(308, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2653] =
            [
                new(4871, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2654] =
            [
                new(4871, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(100, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(4872, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2655] =
            [
                new(452, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2656] =
            [
                new(4871, 3, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(3, new(0, 0, 0, 0, 5, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2657] =
            [
                new(525, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(4876, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2658] =
            [
                new(458, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(640, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2659] =
            [
                new(620, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(4, new(0, 2, 0, 0, 0, 0, 0))]),
                new(170, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(4, new(0, 2, 0, 0, 0, 0, 0))]),
            ],
            [2660] =
            [
                new(450, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2661] =
            [
                new(406, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(330, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2662] =
            [
                new(320, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(4871, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2663] =
            [
                new(4871, 3, 4, 2, 4, 0, 0, 0, 5, 0, false, []),
                new(4872, 3, 4, 2, 4, 0, 0, 0, 5, 0, false, []),
                new(4873, 3, 4, 2, 4, 0, 0, 0, 5, 0, false, []),
            ],
            [2664] =
            [
                new(489, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2665] =
            [
                new(550, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2666] =
            [
                new(374, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2667] =
            [
                new(434, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2668] =
            [
                new(642, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2669] =
            [
                new(415, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2670] =
            [
                new(470, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2671] =
            [
                new(470, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2672] =
            [
                new(113, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2673] =
            [
                new(357, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2674] =
            [
                new(460, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(624, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(459, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2675] =
            [
                new(515, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2676] =
            [
                new(315, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, []),
            ],
            [2677] =
            [
                new(208, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2678] =
            [
                new(111, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(108, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2679] =
            [
                new(321, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(311, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(114, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2680] =
            [
                new(604, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0))]),
            ],
            [2681] =
            [
                new(320, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(108, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2682] =
            [
                new(447, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(318, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2683] =
            [
                new(301, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(299, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(300, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2684] =
            [
                new(605, 20, 20, 2, 4, 0, 0, 0, 5, 0, false, [new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2685] =
            [
                new(470, 10, 15, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(366, 8, 8, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2686] =
            [
                new(319, 10, 10, 2, 4, 0, 0, 0, 5, 0, false, [new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(470, 10, 15, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
                new(445, 6, 6, 2, 4, 0, 0, 0, 5, 0, false, [new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2687] =
            [
                new(470, 10, 15, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(3, 0, 0, 0, 0, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [2397] =
            [
                new(4871, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(2, new(1, 0, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 0, 0, 0, 0))]),
                new(4861, 3, 3, 2, 4, 0, 0, 0, 5, 0, true, [new(2, new(1, 0, 0, 0, 0, 0, 0)), new(4, new(0, 1, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 0, 0, 0, 0))]),
                new(4866, 3, 3, 2, 4, 0, 0, 0, 5, 0, true, [new(2, new(1, 0, 0, 0, 0, 0, 0)), new(4, new(0, 1, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 0, 0, 0, 0))]),
                new(4862, 3, 3, 2, 4, 0, 0, 0, 5, 0, true, [new(2, new(1, 0, 0, 0, 0, 0, 0)), new(4, new(0, 1, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 0, 0, 0, 0))]),
                new(4867, 3, 3, 2, 4, 0, 0, 0, 5, 0, true, [new(2, new(1, 0, 0, 0, 0, 0, 0)), new(4, new(0, 1, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 0, 0, 0, 0))]),
            ],
            [264401] =
            [
                new(192, 5, 5, 2, 4, 0, 0, 0, 5, 0, false, []),
            ],
            [264501] =
            [
                new(193, 3, 3, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(1, 0, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [265301] =
            [
                new(4872, 4, 4, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(4, new(0, 2, 0, 0, 0, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [267001] =
            [
                new(655, 5, 5, 2, 4, 0, 0, 0, 5, 0, false, [new(3, new(0, 0, 0, 0, 5, 0, 0)), new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [267101] =
            [
                new(4872, 5, 5, 2, 4, 0, 0, 0, 5, 0, false, [new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
            [268701] =
            [
                new(4871, 20, 20, 2, 4, 0, 0, 0, 5, 0, false, [new(5, new(0, 0, 1, 1, 0, 0, 0))]),
            ],
        });

    private static ShanheEncounterDefinition WithSource2026EnemyData(ShanheEncounterDefinition encounter, int sourceGroupId)
    {
        var rows = Source2026EnemyDataByGroup[sourceGroupId];
        if (rows.Length != encounter.Enemies.Count)
            throw new InvalidOperationException("A source roster must keep every member and its original order.");
        return encounter with
        {
            SourceVisibleEnemyCount = rows.Count(row => !row.Hidden),
            Enemies = encounter.Enemies.Select((enemy, index) => ApplySource2026EnemyData(enemy, rows[index])).ToArray()
        };
    }

    private static ShanheCombatantDefinition ApplySource2026EnemyData(ShanheCombatantDefinition enemy, Source2026EnemyData row) => enemy with
    {
        Hp = row.Hp, MaxHpBonus = 0, DrawBonus = row.Draw - 2, InitialHandBonus = row.StartingCards - 4,
        SlashBonus = row.ExtraSlash, InitialArmor = row.Armor, ArmorRecoveryPerRound = row.ArmorRecovery,
        ArmorRecoveryCap = row.ArmorCap, ExtraDamage = row.ExtraDamage, SourceDifficultyAttributes = row.TierAttributes
    };

    private static ShanheCombatantDefinition Source2026StageCombatant(int sourceGroupId, string generalId) =>
        ApplySource2026EnemyData(new(generalId), Source2026EnemyDataByGroup[sourceGroupId].Single());
}
