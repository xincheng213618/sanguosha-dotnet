# 移动版伏完

> 此版本与 `ol:fu-wan` 技能规则等价，独立武将定义已删除。本文保留规则和素材来源；处理名单见 [武将技能去重](../GENERAL_RULES_EQUIVALENCE.md)。

核对日期：2026-10-26。正式身份 `mobile:fu-wan`，群、男性、基础四体力，人物 `character:fu-wan`，版本 `mobile`、规则集 `sanguosha-mobile`、画像键 `mobile-fu-wan`；注册与当前将池顺序均为79899。移动Wiki归入SP武将组1，图库使用 `sp`。

[移动官网60](https://www.sanguosha.cn/pc/hero-detail-60.html)、[移动Wiki当前伏完](https://wiki.biligame.com/msgs/伏完)（人物固定版本oldid38597，2026-06-04更新）与[移动Wiki谋溃](https://wiki.biligame.com/msgs/谋溃)的普通身份规则一致：使用杀指定一名角色为目标后，可以选择摸一张牌或弃置该目标一张牌；选择后，只有该角色用闪抵消这次杀，才由该角色弃置伏完一张牌。多目标杀逐目标处理，不能将其他目标的闪或另一张杀移接到本次收益。

完整复用现有 `ol:moukui`，不重新注册同名技能。其定义位于 `src/Characters/fu-wan/ol/Skills/ordinary-fu-wan.rules.json`，由真实 `actualSlashTargetBenefit` 与 `slashDodgeCancelledBenefit` 窗口分别调用原生收益和抵消结算，保留该次杀、该名目标与已选收益的关联。本项只增加人物内容，沿用现有加载和机制检查，没有新增运行时或重复定义快照测试。

此处采用移动专站 `/msgs/` 的经典身份；没有套用十周年的三个选项、双收益选项或杀被无效也反弃的其他版本。当前移动人物页列有合成和招募获取方式，未标为爆料或测试；本次来源核对未发现与上述正式正文冲突的已上线移动重制。

正式经典画像为[官网7200](https://www.sanguosha.cn/storage/uploads/images/skins/7200.jpg)，由官网60原始HTML的皮肤列表首图确认；后两图为7201、7202。经典图HTTP HEAD返回200、image/jpeg、160117字节。建议本地素材 `src/CardGame.Wpf/Assets/official-mobile-fu-wan.jpg`，由批次统一素材与图库接入。
