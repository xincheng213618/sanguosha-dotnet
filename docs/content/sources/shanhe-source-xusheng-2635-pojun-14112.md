# 徐盛2635与专版破军14112

来源为官方 `Config.sgs` 的 `hd_roguelike.sgs`、`cha_spell.sgs` 与 `sys_playcard.sgs`：

- `General.a=2635`，武将587徐盛，基础体力/上限4/4，原技能串 `v=0`，不补普通版破军。
- `getspell_ZD=14112`，`equip_ZD=20633`；中等档6+获得本专版与鬼龙斩月刀。难度成长与固定牌的手牌位置沿共用来源配置接入。
- `Spell.a=14112`，`d=PoJunPoRogue`，原文：当你使用【杀】指定目标后，你可以将其所有手牌移出游戏直到回合结束。你使用的【杀】对手牌区与装备区内牌数皆小于等于你的角色造成的伤害+1。
- `PlayCard.a=20633`，鬼龙斩月刀，花色编码3、点数3；沿已有本地编码约定为梅花3。此固定牌额外加入手牌，不在开局直接装备。

公开资源地址：[Config.sgs](https://web.sanguosha.com/220/h5_2/res/config/Config.sgs)。本批固定缓存外层SHA256为 `d663d5542f5a63a61611b5dbc1ae736e5e0328eedaab2c498e18c3287c289ce6`；三项解码成员分别为：

| 成员 | SHA256 |
| --- | --- |
| hd_roguelike.sgs | 97dbdb465d902e771e575ed5165df3d193e1a689e3dae7ae67b9cb5033f18c72 |
| cha_spell.sgs | e8885a11c46c3b25f37a6fcd8882b0d339ab1d1873c140900d3be93826fdd219 |
| sys_playcard.sgs | 05584b30f7986c06158ee6e47f78417b3d5f1c7fbc61174cfad26e12a5ee64ab |

本地独立技能为 `shanhe:source-pojun-14112`，基础形态与6+技能形态分别为 `shanhe:source-xusheng-2635-base`、`shanhe:source-xusheng-2635-pojun`。发动资格限定山河图战斗；在当前【杀】原生响应前窗口询问，发动后一次移动当前目标全部手牌，装备保持原位。牌放入既有原生 `PojunHold` 离场区，复用其回合结束返还与持牌者死亡清理；实际移牌孩子返回后才继续原【杀】，不会重付手牌。封存不受目标体力值限张。

增伤使用现有 `SourceNotFewerHandAndEquipmentThanTarget` 规则：在原生伤害时分别比较双方当前手牌数量与装备数量，两项均满足才+1；公开伤害预测沿用同一规则。此为独立身份，不替换张辽源3389使用的 `boundary:pojun`，后者仍有按目标体力限张、可选装备的原流程。

新增前向检查以真实实体【杀】、五张大于目标体力的手牌、真实移牌提示孩子与实际回合结束返还核验主链，并以目标多一个装备的场景核验第二项增伤条件。不调用存档、恢复、回放或完整对局结算。
